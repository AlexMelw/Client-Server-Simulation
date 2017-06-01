namespace FlowProtocol.Implementation.Request
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Linq;
    using DomainModels.Entities;
    using Interfaces.Request;
    using MSTranslatorService;
    using Ninject;
    using Storage;
    using static Interfaces.CommonConventions.Conventions;

    public class RequestProcessor : IFlowProtocolRequestProcessor
    {
        private const string AppId = "6CE9C85A41571C050C379F60DA173D286384E0F2";

        private readonly IFlowProtocolRequestParser _parser;

        [Inject]
        public RequestProcessor(IFlowProtocolRequestParser parser)
        {
            _parser = parser;
        }

        public string ProcessRequest(string request)
            {
            ConcurrentDictionary<string, string> requestComponents = _parser.ParseRequest(request);

            if (requestComponents == null)
                return BadRequest;

            if (requestComponents.TryGetValue(Cmd, out string cmd))
            {
                if (cmd == Commands.Hello)
                {
                    return $@"200 OK HELLO";
                }
                if (cmd == Commands.Translate)
                {
                    requestComponents.TryGetValue(SourceText, out string sourceText);
                    requestComponents.TryGetValue(SourceLang, out string sourceLang);
                    requestComponents.TryGetValue(TargetLang, out string targetLang);

                    string translatedText = Translate(
                        sourceText: sourceText,
                        sourceLang: sourceLang,
                        targetLang: targetLang
                    );
                    return $@"200 OK TRANSLATE --res='{translatedText}'";
                }
                if (cmd == Commands.Register)
                {
                    requestComponents.TryGetValue(Login, out string login);
                    requestComponents.TryGetValue(Pass, out string pass);
                    requestComponents.TryGetValue(Name, out string name);

                    if (RegisterUser(login, pass, name))
                    {
                        return $@"200 OK REGISTER --res='User registered successfully'";
                    }

                    return $@"502 ERR REGISTER --res='User already exists'";
                }
                if (cmd == Commands.Auth)
                {
                    requestComponents.TryGetValue(Login, out string login);
                    requestComponents.TryGetValue(Pass, out string pass);

                    Guid authToken = CreateNewSessionForUserWithCredentials(login, pass);

                    if (authToken != Guid.Empty)
                    {
                        return $@"200 OK AUTH --res='User authenticated successfully' --sessiontoken='{authToken}'";
                    }

                    return $@"530 ERR AUTH --res='login or password incorrect'";
                }
                if (cmd == Commands.SendMessage)
                {
                    requestComponents.TryGetValue(SessionToken, out string sessionToken);

                    User senderUser = AuthenticateUser(sessionToken);

                    if (senderUser != null)
                    {
                        requestComponents.TryGetValue(Recipient, out string recipient);

                        if (AuthenticateRecipient(recipient))
                        {
                            requestComponents.TryGetValue(Message, out string message);
                            requestComponents.TryGetValue(SourceLang, out string sourceLang);

                            Debug.Assert(recipient != null, "recipient != null");
                            CorrespondenceManagement.Instance.ClientChatMessageQueues[recipient]
                                .Enqueue(new ChatMessage
                                {
                                    SourceLang = sourceLang,
                                    TextBody = message,
                                    SenderId = senderUser.Login,
                                    SenderName = senderUser.Name
                                });

                            return $@"200 OK SENDMSG --res='Message sent successfully'";
                        }
                        return $@"512 ERR SENDMSG --res='Inexistent recipient'";
                    }
                    return $@"511 ERR SENDMSG --res='Athentication required'";
                }
                if (cmd == Commands.GetMessage)
                {
                    requestComponents.TryGetValue(SessionToken, out string sessionToken);

                    User user = AuthenticateUser(sessionToken);

                    if (user != null)
                    {
                        requestComponents.TryGetValue(TranslationMode, out string translationMode);

                        if (translationMode == DoNotTranslate)
                        {
                            if (CorrespondenceManagement.Instance.ClientChatMessageQueues[user.Login]
                                .TryDequeue(out ChatMessage msg))
                            {
                                return
                                    $"200 OK GETMSG --senderid='{msg.SenderId}' --sendername='{msg.SenderName}' --msg='{msg.TextBody}'";
                            }
                            return $@"513 ERR GETMSG --res='Message Box is empty'";
                        }

                        if (translationMode == DoTranslate)
                        {
                            if (CorrespondenceManagement.Instance.ClientChatMessageQueues[user.Login]
                                .TryDequeue(out ChatMessage msg))
                            {
                                requestComponents.TryGetValue(TargetLang, out string targetLang);

                                string fromLang = msg.SourceLang == Lang.Unknown ? "" : msg.SourceLang;
                                string toLang = targetLang == Lang.Unknown
                                    ? Lang.English
                                    : targetLang;

                                string translatedText;
                                try
                                {
                                    translatedText = Translate(
                                        sourceText: msg.TextBody,
                                        sourceLang: fromLang,
                                        targetLang: toLang);
                                }
                                catch (Exception e)
                                {
                                    translatedText =
                                        "[ Cognitive Services Reply: you have reached your translations limit for today ]";
                                }

                                return
                                    $"200 OK GETMSG --senderid='{msg.SenderId}' --sendername='{msg.SenderName}' --msg='{translatedText}'";
                            }
                            return $@"513 ERR GETMSG --res='Message Box is empty'";
                        }
                    }
                    return $"511 ERR GETMSG --res='Athentication required'";
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
            bool userFound = RegisteredUsers.Instance.Users.TryGetValue(login, out User user);

            if (userFound && user.Pass.Equals(pass))
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
            User newcomer = new User
            {
                Login = login,
                Pass = pass,
                Name = name
            };

            bool registrationSucceded = RegisteredUsers.Instance.TryRegisterUser(newcomer);

            if (registrationSucceded)
            {
                registrationSucceded = CorrespondenceManagement.Instance.TryCreateMailboxForUser(newcomer);
            }

            return registrationSucceded;
        }

        private string Translate(string sourceText, string sourceLang, string targetLang)
        {
            try
            {
                string translatedText;

                using (var translatorClient = new SoapService())
                {
                    string fromLang = sourceLang == Lang.Unknown ? "" : sourceLang;
                    string toLang = targetLang == Lang.Unknown ? Lang.English : targetLang;

                    try
                    {
                        translatedText = translatorClient.Translate(
                            appId: AppId,
                            text: sourceText,
                            from: $"{fromLang}",
                            to: $"{toLang}");
                    }
                    catch (Exception)
                    {
                        translatedText =
                            "[ Cognitive Services Reply: you have reached your translations limit for today ]";
                    }
                }

                return translatedText;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }
    }
}