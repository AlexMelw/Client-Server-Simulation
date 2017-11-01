namespace FlowProtocol.Implementation.Workers.Clients
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Security.Cryptography;
    using DomainModels.Results;
    using Interfaces.CommonConventions;
    using Interfaces.Response;
    using Interfaces.Workers.Clients;
    using ProtocolHelpers;
    using RequestTemplates;
    using Utilities;
    using static Interfaces.CommonConventions.Conventions;

    public class TcpClientWorker : IFlowTcpClientWorker
    {
        private readonly IFlowProtocolResponseParser _parser;

        public RSAParameters ForeignPublicKey { get; set; }
        public RSAParameters OwnPublicKey { get; set; }

        private RSAParameters _ownPrivateKey;

        private TcpClient _client;
        private bool _initialized;

        private Guid _sessionToken = Guid.Empty;

        public int Port { get; private set; }
        public IPAddress RemoteHostIpAddress { get; private set; }

        #region CONSTRUCTORS

        public TcpClientWorker(IFlowProtocolResponseParser parser)
        {
            _parser = parser;
            GenerateRsaKeyPair();
        }

        private void GenerateRsaKeyPair()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                OwnPublicKey = rsa.ExportParameters(false);
                _ownPrivateKey = rsa.ExportParameters(true);
            }
        }

        #endregion

        public bool Connect(IPAddress ipAddress, int port)
        {
            RemoteHostIpAddress = ipAddress;
            Port = port;

            _initialized = false;

            try
            {
                _client = new TcpClient();
                _client.Connect(RemoteHostIpAddress, Port);

                NetworkStream networkStream = _client.GetStream();

                byte[] buffer = Commands.Hello.ToFlowProtocolAsciiEncodedBytesArray();


                if (networkStream.CanWrite)
                {
                    networkStream.Write(buffer, FromBeginning, buffer.Length);
                    networkStream.Flush();
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
                        _initialized = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                _initialized = false;
                throw new Exception("No connection to server");
            }
            finally
            {
                _client.Close();
            }
            return false;
        }

        public bool Authenticate(string login, string password)
        {
            if (_initialized == false)
            {
                throw new Exception("No connection to server");
            }

            _client = new TcpClient();

            try
            {
                _client.Connect(RemoteHostIpAddress, Port);

                NetworkStream networkStream = _client.GetStream();

                string textToBeSent = string.Format(Template.AuthenticationTemplate, login, password);

                byte[] buffer = textToBeSent.ToFlowProtocolAsciiEncodedBytesArray();

                if (networkStream.CanWrite)
                {
                    networkStream.Write(buffer, FromBeginning, buffer.Length);
                    networkStream.Flush();
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

        public bool Register(string login, string password, string name)
        {
            if (_initialized == false)
            {
                throw new Exception("No connection to server");
            }

            _client = new TcpClient();

            try
            {
                _client.Connect(RemoteHostIpAddress, Port);

                NetworkStream networkStream = _client.GetStream();

                string textToBeSent = string.Format(Template.RegisterTemplate, login, password, name);

                byte[] buffer = textToBeSent.ToFlowProtocolAsciiEncodedBytesArray();

                if (networkStream.CanWrite)
                {
                    networkStream.Write(buffer, FromBeginning, buffer.Length);
                    networkStream.Flush();
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
                throw new Exception("No connection to server");
            }

            _client = new TcpClient();

            try
            {
                _client.Connect(RemoteHostIpAddress, Port);

                NetworkStream networkStream = _client.GetStream();

                // From "English" to "en", from "Romanian" to "ro", etc.
                FlowUtility.ConvertToFlowProtocolLanguageNotations(ref sourceTextLang);
                FlowUtility.ConvertToFlowProtocolLanguageNotations(ref targetTextLanguage);

                string textToBeSent = string.Format(Template.TranslateTemplate,
                    sourceText, sourceTextLang, targetTextLanguage);

                byte[] buffer = textToBeSent.ToFlowProtocolAsciiEncodedBytesArray();

                if (networkStream.CanWrite)
                {
                    networkStream.Write(buffer, FromBeginning, buffer.Length);
                    networkStream.Flush();
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

        public SendMessageResult SendMessage(string recipient, string messageText, string messageTextLang)
        {
            if (_initialized == false)
            {
                throw new Exception("No connection to server");
            }

            if (_sessionToken == Guid.Empty)
            {
                throw new Exception("Not authorized. You have to sign in first.");
            }

            _client = new TcpClient();

            try
            {
                _client.Connect(RemoteHostIpAddress, Port);

                NetworkStream networkStream = _client.GetStream();

                // From "English" to "en", from "Romanian" to "ro", etc.
                FlowUtility.ConvertToFlowProtocolLanguageNotations(ref messageTextLang);

                string textToBeSent = string.Format(Template.SendMessageTemplate,
                    recipient, messageText, messageTextLang, _sessionToken);

                byte[] buffer = textToBeSent.ToFlowProtocolAsciiEncodedBytesArray();

                if (networkStream.CanWrite)
                {
                    networkStream.Write(buffer, FromBeginning, buffer.Length);
                    networkStream.Flush();
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
                    if (cmd == Commands.SendMessage)
                    {
                        if (responseComponents.TryGetValue(StatusDescription, out string statusDesc))
                        {
                            if (statusDesc == Error)
                            {
                                return new SendMessageResult
                                {
                                    Success = false
                                };
                            }
                            if (statusDesc == Ok)
                            {
                                if (responseComponents.TryGetValue(ResultValue, out string resultValue))
                                {
                                    return new SendMessageResult
                                    {
                                        Success = true,
                                        ResponseMessage = resultValue
                                    };
                                }
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
            return new SendMessageResult
            {
                Success = false
            };
        }

        public GetMessageResult GetMessage(string translationMode)
        {
            if (_initialized == false)
            {
                throw new Exception("No connection to server");
            }

            if (_sessionToken == Guid.Empty)
            {
                throw new Exception("Not authorized. You have to sign in first.");
            }

            _client = new TcpClient();

            try
            {
                _client.Connect(RemoteHostIpAddress, Port);

                NetworkStream networkStream = _client.GetStream();

                if (translationMode == Template.Convention.ClientSaysDoNotTranslate)
                {
                    string textToBeSent = string.Format(Template.GetMessageUnmodifiedTemplate,
                        _sessionToken);

                    byte[] buffer = textToBeSent.ToFlowProtocolAsciiEncodedBytesArray();

                    if (networkStream.CanWrite)
                    {
                        networkStream.Write(buffer, FromBeginning, buffer.Length);
                        networkStream.Flush();
                    }
                }
                else
                {
                    // From "English" to "en", from "Romanian" to "ro", etc.
                    FlowUtility.ConvertToFlowProtocolLanguageNotations(ref translationMode);

                    string textToBeSent = string.Format(Template.GetMessageTranslatedTemplate,
                        _sessionToken, translationMode);

                    byte[] buffer = textToBeSent.ToFlowProtocolAsciiEncodedBytesArray();

                    if (networkStream.CanWrite)
                    {
                        networkStream.Write(buffer, FromBeginning, buffer.Length);
                        networkStream.Flush();
                    }
                }

                string response = string.Empty;

                if (networkStream.CanRead)
                {
                    byte[] buffer = new byte[EthernetTcpUdpPacketSize];
                    int bytesRead = networkStream.Read(buffer, FromBeginning, EthernetTcpUdpPacketSize);
                    response = buffer.Take(bytesRead).ToArray().ToFlowProtocolAsciiDecodedString();
                }

                var responseComponents = _parser.ParseResponse(response);

                if (responseComponents.TryGetValue(Cmd, out string cmd))
                {
                    if (cmd == Commands.GetMessage)
                    {
                        if (responseComponents.TryGetValue(StatusDescription, out string statusDesc))
                        {
                            if (statusDesc == Error)
                            {
                                responseComponents.TryGetValue(Conventions.ResultValue, out string resultValue);

                                return new GetMessageResult
                                {
                                    Success = false,
                                    ErrorExplained = resultValue
                                };
                            }
                            if (statusDesc == Ok)
                            {
                                responseComponents.TryGetValue(SenderId, out string senderId);
                                responseComponents.TryGetValue(SenderName, out string senderName);
                                responseComponents.TryGetValue(Message, out string message);

                                return new GetMessageResult
                                {
                                    Success = true,
                                    SenderId = senderId,
                                    SenderName = senderName,
                                    MessageBody = message
                                };
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
            return new GetMessageResult
            {
                Success = false
            };
        }

        public void Dispose() => ((IDisposable) _client)?.Dispose();
    }
}