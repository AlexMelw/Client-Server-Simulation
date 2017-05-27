namespace Protocol.Implementation.Workers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using EasySharp.NHelpers;
    using Interfaces;
    using Interfaces.CommonConventions;
    using Interfaces.ProtocolHelpers;
    using Interfaces.Response;

    public class TcpClientWorker : IClientWorker
    {
        private const int FromBeginning = 0;
        private const int EthernetTcpUdpPacketSize = 1472;

        private const string AuthenticationFormat =
            @"AUTH --clienttype='tcp' --listenport='0' --login='{0}' --pass='{1}'";

        private TcpClient _client;
        private string _login;
        private string _password;
        private IFlowProtocolResponseProcessor _responseProcessor;
        public int Port { get; private set; }
        public IPAddress RemoteHostIpAddress { get; private set; }
        public string TextToBeSent { get; set; } = string.Empty;

        #region CONSTRUCTORS

        public TcpClientWorker(IFlowProtocolResponseProcessor responseProcessor)
        {
            _responseProcessor = responseProcessor;
        }

        #endregion

        public void Init(IPAddress ipAddress, int port)
        {
            RemoteHostIpAddress = ipAddress;
            Port = port;
        }

        public void StartCommunication()
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

                while (!TextToBeSent.Equals(Conventions.CloseConnection))
                {
                    string textToBeSent = Console.ReadLine();
                    NetworkStream networkStream = _client.GetStream();
                    byte[] bufferBytesArray = textToBeSent.ToAsciiEncodedByteArray();

                    Console.Out.WriteLine($"Transmitting [ {textToBeSent} ]");

                    if (networkStream.CanWrite)
                    {
                        networkStream.Write(bufferBytesArray, Conventions.FromBeginning, bufferBytesArray.Length);
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

        public string Authenticate(string login, string password)
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
                tcpStream.Write(bufferBytesArray, Conventions.FromBeginning, bufferBytesArray.Length);
                tcpStream.Flush(); // ???
            }

            bufferBytesArray = new byte[Conventions.EthernetTcpUdpPacketSize];

            if (tcpStream.CanRead)
            {
                int bytesRead = tcpStream.Read(
                    buffer: bufferBytesArray,
                    offset: Conventions.FromBeginning,
                    size: Conventions.EthernetTcpUdpPacketSize);

                string serverResponse = bufferBytesArray.Take(bytesRead)
                    .ToArray()
                    .ToFlowProtocolAsciiDecodedString();

                if (_responseProcessor.IsAuthenticated(serverResponse))
                {
                    _login = login;
                    _password = password;
                    return Conventions.OK;
                }
                else
                {
                    return Conventions.NotAuthenticated;
                }
            }
            else
            {
                Console.Out.WriteLine("Error. Can't receive response from server.");
                _client.Close();
            }


            return Conventions.OK;
        }

        public void Send(string message)
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
                tcpStream.Write(bufferBytesArray, Conventions.FromBeginning, bufferBytesArray.Length);
                tcpStream.Flush(); // ???
            }


            bufferBytesArray = new byte[Conventions.EthernetTcpUdpPacketSize];

            if (tcpStream.CanRead)
            {
                int bytesRead = tcpStream.Read(
                    bufferBytesArray,
                    Conventions.FromBeginning,
                    Conventions.EthernetTcpUdpPacketSize);

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

        public void Init(string ipAddress, int port)
        {
            Init(IPAddress.Parse(ipAddress), port);
        }
    }
}