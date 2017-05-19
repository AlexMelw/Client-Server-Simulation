namespace Presentation.Console.ClientApp
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using EasySharp.NHelpers;
    using Protocol.Interfaces;

    public class TcpWorker : IWorker
    {
        private const int FromBeginning = 0;
        private const int EthernetTcpUdpPacketSize = 1472;
        private TcpClient _client;
        public int Port { get; private set; }
        public IPAddress RemoteHostIpAddress { get; private set; }

        public void Init(string ipAddress, int port)
        {
            Init(IPAddress.Parse(ipAddress), port);
        }

        public void Init(IPAddress ipAddress, int port)
        {
            RemoteHostIpAddress = ipAddress;
            Port = port;
            _client = new TcpClient();
        }

        public void StartCommunication()
        {
            new Thread(() =>
            {
                Console.Out.WriteLine("Connecting to server");
                _client.Connect(RemoteHostIpAddress, Port);
                Console.Out.WriteLine($"Connected to {RemoteHostIpAddress}:{Port}");

                for (;;)
                {
                    string textToBeSent = Console.ReadLine();
                    NetworkStream networkStream = _client.GetStream();
                    byte[] bufferBytesArray = textToBeSent.GetAsciiEncodedByteArray();

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

        public void Send(string message)
        {
            NetworkStream networkStream = _client.GetStream();
            byte[] bufferBytesArray = message.GetAsciiEncodedByteArray();

            Console.Out.WriteLine($"Transmitting [ {message} ]");

            if (networkStream.CanWrite)
            {
                networkStream.Write(bufferBytesArray, FromBeginning, bufferBytesArray.Length);
            }
        }
    }
}