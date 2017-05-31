namespace FlowProtocol.Implementation.Workers.Clients
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using Interfaces.Response;
    using Interfaces.Workers;
    using ProtocolHelpers;
    using Results;
    using static Interfaces.CommonConventions.Conventions;
    using static Interfaces.CommonConventions.Conventions.Commands;

    public class UdpClientWorker : IFlowClientWorker
    {
        private readonly IFlowProtocolResponseParser _parser;

        private UdpClient _client;
        private bool _initialized;
        private string _login;
        private string _password;

        private Guid _sessionToken = Guid.Empty;

        public int Port { get; private set; }
        public IPAddress RemoteHostIpAddress { get; private set; }

        #region CONSTRUCTORS

        public UdpClientWorker(IFlowProtocolResponseParser parser)
        {
            _parser = parser;
        }

        #endregion

        public bool Authenticate(string login, string password)
        {
            throw new NotImplementedException();
        }

        public bool Connect(IPAddress ipAddress, int port)
        {
            RemoteHostIpAddress = ipAddress;
            Port = port;

            _initialized = true;

            try
            {
                var serverEndPoint = new IPEndPoint(RemoteHostIpAddress, Port);

                _client = new UdpClient();
                _client.Connect(serverEndPoint);

                byte[] buffer = Hello.ToFlowProtocolAsciiEncodedBytesArray();
                _client.Send(buffer, buffer.Length);

                buffer = _client.Receive(ref serverEndPoint);

                string response = string.Empty;

                if (buffer.Length > 0)
                {
                    response = buffer.ToFlowProtocolAsciiDecodedString();
                }

                var responseComponents = _parser.ParseResponse(response);

                if (responseComponents.TryGetValue(Cmd, out string cmd))
                {
                    if (cmd.Equals(Hello))
                    {
                        return true;
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
            finally
            {
                _client.Close();
            }
            return false;
        }

        public GetMessageResult GetMessage(string translationMode)
        {
            throw new NotImplementedException();
        }

        public bool Register(string login, string password, string name)
        {
            throw new NotImplementedException();
        }

        public SendMessageResult SendMessage(string recipient, string messageText, string messageTextLang)
        {
            throw new NotImplementedException();
        }

        public string Translate(string sourceText, string sourceTextLang, string targetTextLanguage)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => ((IDisposable) _client)?.Dispose();
    }
}