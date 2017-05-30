namespace FlowProtocol.Implementation.Workers.Clients
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using Interfaces.CommonConventions;
    using Interfaces.Response;
    using Interfaces.Workers;
    using ProtocolHelpers;
    using static Interfaces.CommonConventions.Conventions;

    public class TcpClientWorker : IFlowClientWorker
    {
        //private const string AuthenticationFormat =
        //    @"AUTH --clienttype='tcp' --listenport='0' --login='{0}' --pass='{1}'";

        private readonly IFlowProtocolResponseParser _parser;
        private TcpClient _client;

        private Guid _sessionToken = Guid.Empty;
        private string _login;
        private string _password;
        private bool _initialized;
        private readonly string AuthenticationTemplate = @"AUTH  --login='{0}' --pass='{1}'";

        public TcpClientWorker(IFlowProtocolResponseParser parser)
        {
            _parser = parser;
        }

        public int Port { get; private set; }
        public IPAddress RemoteHostIpAddress { get; private set; }

        public bool TryConnect(IPAddress ipAddress, int port)
        {
            RemoteHostIpAddress = ipAddress;
            Port = port;

            _initialized = true;

            _client = new TcpClient();
            _client.Connect(RemoteHostIpAddress, Port);

            NetworkStream networkStream = _client.GetStream();

            byte[] buffer = Hello.ToFlowProtocolAsciiEncodedBytesArray();


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

            _client.Close();

            if (response.Equals(Hello))
            {
                return true;
            }
            return false;
        }

        public bool TryAuthenticate(string login, string password)
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
                    if (cmd == Commands.Register)
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

        public string Translate(string sourceText, string sourceTextLang, string targetTextLanguage)
        {
            throw new NotImplementedException();
        }

        public void InitZZZZZZZZZZZZZZZ(IPAddress ipAddress, int port)
        {
            RemoteHostIpAddress = ipAddress;
            Port = port;
        }

        public void StartCommunicationZZZZZZZZZZZZZ()
        {
            if (_client == null)
            {
                throw new Exception($"{nameof(_client)} is not initialized");
            }

            new Thread(() =>
            {
                Console.Out.WriteLine("Connecting to server...");
                _client.Connect(RemoteHostIpAddress, Port);
                Console.Out.WriteLine($"Connected to {RemoteHostIpAddress} : {Port}");

                while (!TextToBeSent.Equals(CloseConnection))
                {
                    string textToBeSent = Console.ReadLine();
                    NetworkStream networkStream = _client.GetStream();
                    // byte[] bufferBytesArray = textToBeSent.ToAsciiEncodedByteArray();

                    Console.Out.WriteLine($"Transmitting [ {textToBeSent} ]");

                    if (networkStream.CanWrite)
                    {
                        networkStream.Write(bufferBytesArray, FromBeginning, bufferBytesArray.Length);
                    }

                    string response = string.Empty;
                    if (networkStream.CanRead)
                    {
                        bufferBytesArray = new byte[EthernetTcpUdpPacketSize];
                        int bytesRead = networkStream.Read(bufferBytesArray, FromBeginning, EthernetTcpUdpPacketSize);
                        response = bufferBytesArray.Take(bytesRead).ToArray().ToAsciiString();
                    }
                }
                _client.Close();
            }).Start();
        }

        public string AuthenticateZZZZZZZZZZZZZZZZ(string login, string password)
        {
            _client = _client ?? new TcpClient();

            Console.Out.WriteLine($"Connecting to server {RemoteHostIpAddress} : {Port}");

            _client.Connect(RemoteHostIpAddress, Port);

            if (!_client.Connected)
            {
                Console.Out.WriteLine("Client is not connected to server");
                _client.Close();
                return $"Registration failed. No connection to server.";
            }

            Console.Out.WriteLine($"Connected successfully");

            NetworkStream tcpStream = _client.GetStream();
            string authenticationRequest = string.Format(AuthenticationFormat, _login, _password);
            byte[] bufferBytesArray = authenticationRequest.ToFlowProtocolAsciiEncodedBytesArray();

            if (tcpStream.CanWrite)
            {
                Console.WriteLine(" Transmitting.....");
                tcpStream.Write(bufferBytesArray, FromBeginning, bufferBytesArray.Length);
                tcpStream.Flush(); // ???
            }

            bufferBytesArray = new byte[EthernetTcpUdpPacketSize];

            if (tcpStream.CanRead)
            {
                int bytesRead = tcpStream.Read(
                    bufferBytesArray,
                    FromBeginning,
                    EthernetTcpUdpPacketSize);

                string serverResponse = bufferBytesArray.Take(bytesRead)
                    .ToArray()
                    .ToFlowProtocolAsciiDecodedString();

                if (_responseProcessor.IsAuthenticated(serverResponse))
                {
                    _login = login;
                    _password = password;
                    return Conventions.OK;
                }
                return NotAuthenticated;
            }
            Console.Out.WriteLine("Error. Can't receive response from server.");
            _client.Close();


            return Conventions.OK;
        }

        public void SendZZZZZZZZZZZZZZZZZ(string message)
        {
            _client = _client ?? new TcpClient();

            Console.Out.WriteLine($"Connecting to server {RemoteHostIpAddress} : {Port}");

            _client.Connect(RemoteHostIpAddress, Port);

            if (!_client.Connected)
            {
                Console.Out.WriteLine("Client is not connected to server");
                _client.Close();
                return $"Registration failed. No connection to server.";
            }

            Console.Out.WriteLine($"Connected successfully");

            NetworkStream tcpStream = _client.GetStream();
            string authenticationRequest = string.Format(AuthenticationFormat, _login, _password);
            byte[] bufferBytesArray = authenticationRequest.ToFlowProtocolAsciiEncodedBytesArray();

            if (tcpStream.CanWrite)
            {
                Console.WriteLine(" Transmitting.....");
                tcpStream.Write(bufferBytesArray, FromBeginning, bufferBytesArray.Length);
                tcpStream.Flush(); // ???
            }


            bufferBytesArray = new byte[EthernetTcpUdpPacketSize];

            if (tcpStream.CanRead)
            {
                int bytesRead = tcpStream.Read(
                    bufferBytesArray,
                    FromBeginning,
                    EthernetTcpUdpPacketSize);

                string serverResponse = bufferBytesArray.Take(bytesRead)
                    .ToArray()
                    .ToFlowProtocolAsciiDecodedString();
            }
            else
            {
                _client.Close();
            }


            //_responseProcessor
        }

        public void Dispose()
        {
            ((IDisposable) _client)?.Dispose();
        }
    }
}