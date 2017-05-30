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
    using static Interfaces.CommonConventions.Conventions;

    public class TcpClientWorker : IFlowClientWorker
    {
        //private const string AuthenticationFormat =
        //    @"AUTH --clienttype='tcp' --listenport='0' --login='{0}' --pass='{1}'";

        private readonly IFlowProtocolResponseParser _parser;
        private readonly string AuthenticationTemplate = @"AUTH  --login='{0}' --pass='{1}'";
        private readonly string RegisterTemplate = @"REGISTER  --login='{0}' --pass='{1}' --name='{2}'";

        private readonly string TranslateTemplate =
                @"TRANSLATE  --sourcetext='{0}' --sourcelang='{1}' --targetlang='{2}'"
            ;

        private TcpClient _client;
        private bool _initialized;
        private string _login;
        private string _password;

        private Guid _sessionToken = Guid.Empty;

        public int Port { get; private set; }
        public IPAddress RemoteHostIpAddress { get; private set; }

        #region CONSTRUCTORS

        public TcpClientWorker(IFlowProtocolResponseParser parser)
        {
            _parser = parser;
        }

        #endregion

        public bool TryConnect(IPAddress ipAddress, int port)
        {
            RemoteHostIpAddress = ipAddress;
            Port = port;

            _initialized = true;

            try
            {
                _client = new TcpClient();
                _client.Connect(RemoteHostIpAddress, Port);

                NetworkStream networkStream = _client.GetStream();

                byte[] buffer = Commands.Hello.ToFlowProtocolAsciiEncodedBytesArray();


                if (networkStream.CanWrite)
                {
                    networkStream.Write(buffer, FromBeginning, buffer.Length);
                }

                string response = string.Empty;

                if (networkStream.CanRead)
                {
                    buffer = new byte[EthernetTcpUdpPacketSize];
                    int bytesRead = networkStream.Read(buffer, FromBeginning, EthernetTcpUdpPacketSize);
                    response = buffer.Take(bytesRead).ToArray().ToFlowProtocolAsciiDecodedString();
                }

                var responseComponents = _parser.ParseResponse(response);

                if (responseComponents.TryGetValue(Cmd, out string cmd))
                {
                    if (cmd.Equals(Commands.Hello))
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

        public bool TryAuthenticate(string login, string password)
        {
            if (_initialized == false)
            {
                return false;
            }

            _login = login;
            _password = password;

            _client = new TcpClient();

            try
            {
                _client.Connect(RemoteHostIpAddress, Port);

                NetworkStream networkStream = _client.GetStream();

                string textToBeSent = string.Format(AuthenticationTemplate, login, password);

                byte[] buffer = textToBeSent.ToFlowProtocolAsciiEncodedBytesArray();

                if (networkStream.CanWrite)
                {
                    networkStream.Write(buffer, FromBeginning, buffer.Length);
                }

                string response = string.Empty;

                if (networkStream.CanRead)
                {
                    buffer = new byte[EthernetTcpUdpPacketSize];
                    int bytesRead = networkStream.Read(buffer, FromBeginning, EthernetTcpUdpPacketSize);
                    response = buffer.Take(bytesRead).ToArray().ToFlowProtocolAsciiDecodedString();
                }

                var responseComponents = _parser.ParseResponse(response);

                if (responseComponents.TryGetValue(Cmd, out string cmd))
                {
                    if (cmd == Commands.Auth)
                    {
                        if (responseComponents.TryGetValue(StatusDescription, out string statusDesc))
                        {
                            if (statusDesc == Error)
                            {
                                return false;
                            }
                            if (responseComponents.TryGetValue(SessionToken, out string token))
                            {
                                _sessionToken = Guid.Parse(token);
                                return true;
                            }
                        }
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

        public bool TryRegister(string login, string password, string name)
        {
            if (_initialized == false)
            {
                return false;
            }

            _client = new TcpClient();

            try
            {
                _client.Connect(RemoteHostIpAddress, Port);

                NetworkStream networkStream = _client.GetStream();

                string textToBeSent = string.Format(RegisterTemplate, login, password, name);

                byte[] buffer = textToBeSent.ToFlowProtocolAsciiEncodedBytesArray();

                if (networkStream.CanWrite)
                {
                    networkStream.Write(buffer, FromBeginning, buffer.Length);
                }

                string response = string.Empty;

                if (networkStream.CanRead)
                {
                    buffer = new byte[EthernetTcpUdpPacketSize];
                    int bytesRead = networkStream.Read(buffer, FromBeginning, EthernetTcpUdpPacketSize);
                    response = buffer.Take(bytesRead).ToArray().ToFlowProtocolAsciiDecodedString();
                }

                var responseComponents = _parser.ParseResponse(response);

                if (responseComponents.TryGetValue(Cmd, out string cmd))
                {
                    if (cmd == Commands.Register)
                    {
                        if (responseComponents.TryGetValue(StatusDescription, out string statusDesc))
                        {
                            if (statusDesc == Error)
                            {
                                return false;
                            }
                            if (statusDesc == Ok)
                            {
                                return true;
                            }
                        }
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

        public string Translate(string sourceText, string sourceTextLang, string targetTextLanguage)
        {
            if (_initialized == false)
            {
                return string.Empty;
            }

            _client = new TcpClient();

            try
            {
                _client.Connect(RemoteHostIpAddress, Port);

                NetworkStream networkStream = _client.GetStream();

                // From "English" to "en", from "Romanian" to "ro", etc.
                ConvertToFlowLangNotations(ref sourceTextLang, ref targetTextLanguage);

                string textToBeSent = string.Format(TranslateTemplate,
                    sourceText, sourceTextLang, targetTextLanguage);

                byte[] buffer = textToBeSent.ToFlowProtocolAsciiEncodedBytesArray();

                if (networkStream.CanWrite)
                {
                    networkStream.Write(buffer, FromBeginning, buffer.Length);
                }

                string response = string.Empty;

                if (networkStream.CanRead)
                {
                    buffer = new byte[EthernetTcpUdpPacketSize];
                    int bytesRead = networkStream.Read(buffer, FromBeginning, EthernetTcpUdpPacketSize);
                    response = buffer.Take(bytesRead).ToArray().ToFlowProtocolAsciiDecodedString();
                }

                var responseComponents = _parser.ParseResponse(response);

                if (responseComponents.TryGetValue(Cmd, out string cmd))
                {
                    if (cmd == Commands.Translate)
                    {
                        if (responseComponents.TryGetValue(ResultValue, out string resultValue))
                        {
                            return resultValue;
                        }
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
            return string.Empty;
        }

        public void Dispose()
        {
            ((IDisposable) _client)?.Dispose();
        }

        public void ConvertToFlowLangNotations(ref string sourceTextLang, ref string targetTextLanguage)
        {
            const string english = "English";
            const string romanian = "Romanian";
            const string russian = "Russian";
            const string autoDetection = "Auto Detection";

            const string ro = "ro";
            const string ru = "ru";
            const string en = "en";
            const string unknown = "unknown";

            switch (sourceTextLang)
            {
                case english:
                    sourceTextLang = en;
                    break;
                case romanian:
                    sourceTextLang = ro;
                    break;
                case russian:
                    sourceTextLang = ru;
                    break;
                case autoDetection:
                    sourceTextLang = unknown;
                    break;
            }
            switch (targetTextLanguage)
            {
                case english:
                    targetTextLanguage = en;
                    break;
                case romanian:
                    targetTextLanguage = ro;
                    break;
                case russian:
                    targetTextLanguage = ru;
                    break;
            }
        }
    }
}