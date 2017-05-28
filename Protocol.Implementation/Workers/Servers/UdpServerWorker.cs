namespace FlowProtocol.Implementation.Workers.Servers
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using EasySharp.NHelpers;
    using Entities;
    using Interfaces;
    using Interfaces.Request;
    using MSTranslatorService;
    using Ninject;
    using Request;
    using Storage;
    using static Interfaces.CommonConventions.Conventions;

    public class UdpServerWorker : IFlowServerWorker
    {
        private const string AppId = "6CE9C85A41571C050C379F60DA173D286384E0F2";

        // TODO From IFlowProtocolRequestParser to IFlowProtocolRequestProcessor to avoid code copy-paste (UDP & TCP workers)

        private readonly IFlowProtocolRequestParser _parser;
        private IPEndPoint _remoteClientEndPoint;
        private UdpClient _udpServer;
        public static UdpServerWorker Instance => new UdpServerWorker(new RequestParser());

        #region CONSTRUCTORS

        [Inject]
        public UdpServerWorker(IFlowProtocolRequestParser parser)
        {
            _parser = parser;
        }

        #endregion

        public void ProcessRequest(string request)
        {
            new Thread(() =>
            {
                Console.Out.WriteLine($"[ UDP ] SERVER WORKER IS TALKING TO {_remoteClientEndPoint}");

                string result = ExecuteRequest(request);

                byte[] bufferByteArray = result.ToAsciiEncodedByteArray();

                _udpServer.Send(bufferByteArray, bufferByteArray.Length, _remoteClientEndPoint);

                Console.Out.WriteLine($"[ UDP ] SERVER WORKER for {_remoteClientEndPoint} finished job");
            }).Start();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        private string ExecuteRequest(string request)
        {
            ConcurrentDictionary<string, string> requestMembers = _parser.ParseRequest(request);

            if (requestMembers == null)
                return BadRequest;

            if (requestMembers.TryGetValue(Cmd, out string cmd))
            {
                if (cmd == Commands.Translate)
                {
                    requestMembers.TryGetValue(SourceText, out string sourceText);
                    requestMembers.TryGetValue(SourceLang, out string sourceLang);
                    requestMembers.TryGetValue(TargetLang, out string targetLang);

                    string translatedText = Translate(
                        sourceText,
                        sourceLang,
                        targetLang
                    );
                    return $@"200 OK TRANSLATE --TEXT='{translatedText}'";
                }
                if (cmd == Commands.Register)
                {
                    requestMembers.TryGetValue(Login, out string login);
                    requestMembers.TryGetValue(Pass, out string pass);
                    requestMembers.TryGetValue(Name, out string name);

                    if (RegisterUser(login, pass, name))
                    {
                        return $@"200 OK REGISTER --RES='User registered successfully'";
                    }

                    return $@"502 ERR REGISTER --RES='User already exists'";
                }
                if (cmd == Commands.Auth)
                {
                    requestMembers.TryGetValue(Login, out string login);
                    requestMembers.TryGetValue(Pass, out string pass);

                    Guid authToken = CreateNewSessionForUserWithCredentials(login, pass);

                    if (authToken != Guid.Empty)
                    {
                        return $@"200 OK AUTH --RES='User authenticated successfully' AuthToken='{authToken}'";
                    }

                    return $@"530 ERR AUTH --RES='login or password incorrect'";
                }
                if (cmd == Commands.SendMessage)
                {
                    requestMembers.TryGetValue(SessionToken, out string sessionToken);

                    User senderUser = AuthenticateUser(sessionToken);

                    if (senderUser != null)
                    {
                        requestMembers.TryGetValue(Recipient, out string recipient);

                        if (AuthenticateRecipient(recipient))
                        {
                            requestMembers.TryGetValue(Message, out string message);
                            requestMembers.TryGetValue(SourceLang, out string sourceLang);

                            Debug.Assert(recipient != null, "recipient != null");
                            CorrespondenceManagement.Instance.ClientChatMessageQueues[recipient]
                                .Enqueue(new ChatMessage
                                {
                                    SourceLang = sourceLang,
                                    TextBody = message,
                                    SenderId = senderUser.Login,
                                    SenderName = senderUser.Name
                                });

                            return $@"200 OK SENDMSG --RES='Message sent successfully'";
                        }
                        return $@"512 ERR SENDMSG --RES='Inexistent recipient'";
                    }
                    return $@"511 ERR SENDMSG --RES='Athentication required'";
                }
                if (cmd == Commands.GetMessage)
                {
                    requestMembers.TryGetValue(SessionToken, out string sessionToken);

                    User user = AuthenticateUser(sessionToken);

                    if (user != null)
                    {
                        requestMembers.TryGetValue(TranslationMode, out string translationMode);

                        if (translationMode == DoNotTranslate)
                        {
                            if (CorrespondenceManagement.Instance.ClientChatMessageQueues[user.Login]
                                .TryDequeue(out ChatMessage msg))
                            {
                                return
                                    $"200 OK GETMSG --senderid='{msg.SenderId}' --sendername='{msg.SenderName}' --text='{msg.TextBody}'";
                            }
                            return $@"513 ERR GETMSG --RES='Message Box is empty'";
                        }

                        if (translationMode == DoTranslate)
                        {
                            if (CorrespondenceManagement.Instance.ClientChatMessageQueues[user.Login]
                                .TryDequeue(out ChatMessage msg))
                            {
                                requestMembers.TryGetValue(TargetLang, out string targetLang);

                                string translatedText = Translate(
                                    sourceText: msg.TextBody,
                                    sourceLang: msg.SourceLang,
                                    targetLang: targetLang);

                                return
                                    $"200 OK GETMSG --senderid='{msg.SenderId}' --sendername='{msg.SenderName}' --text='{translatedText}'";
                            }
                            return $@"513 ERR GETMSG --RES='Message Box is empty'";
                        }
                    }
                    return $"511 ERR GETMSG --RES='Athentication required'";
                }
            }
            return BadRequest;
        }

        private bool AuthenticateRecipient(string recipient)
        {
            bool found = RegisteredUsers.Instance.Users.ContainsKey(recipient);

            return found;
        }

        private User AuthenticateUser(string sessionToken)
        {
            Guid token = Guid.Parse(sessionToken);

            AuthClient authClient = AuthenticatedClients.Instance.Clients.Values
                .FirstOrDefault(client => client.AuthToken.Equals(token));

            return authClient?.User;
        }

        private Guid CreateNewSessionForUserWithCredentials(string login, string pass)
        {
            if (RegisteredUsers.Instance.Users.TryGetValue(login, out User user))
            {
                AuthenticatedClients.Instance.Clients.AddOrUpdate(
                    key: login,
                    addValue: new AuthClient
                    {
                        User = user,
                        AuthToken = Guid.NewGuid()
                    },
                    updateValueFactory: (keyLogin, authClient) =>
                    {
                        authClient.AuthToken = Guid.NewGuid();
                        return authClient;
                    }
                );

                return AuthenticatedClients.Instance.Clients.GetOrAdd(login, AuthClient.Empty).AuthToken;
            }

            return Guid.Empty;
        }

        private bool RegisterUser(string login, string pass, string name)
        {
            return RegisteredUsers.Instance.Users.TryAdd(login, new User
            {
                Login = login,
                Pass = pass,
                Name = name
            });
        }

        public string Translate(string sourceText, string sourceLang, string targetLang)
        {
            try
            {
                var translatorClient = new SoapService();

                string from = sourceLang == Lang.Unknown ? "" : sourceLang;
                string to = targetLang == Lang.Unknown ? Lang.English : targetLang;

                string translatedText = translatorClient.Translate(
                    appId: "6CE9C85A41571C050C379F60DA173D286384E0F2",
                    text: sourceText,
                    from: $"{from}",
                    to: $"{to}");

                return translatedText;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        ~UdpServerWorker()
        {
            ReleaseUnmanagedResources();
        }

        public UdpServerWorker Init(IPEndPoint remoteClientEndPoint, UdpClient udpServer)
        {
            _remoteClientEndPoint = remoteClientEndPoint;
            _udpServer = udpServer;
            return this;
        }
    }
}