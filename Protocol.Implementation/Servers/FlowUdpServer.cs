namespace FlowProtocol.Implementation.Servers
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using EasySharp.NHelpers;
    using Interfaces;
    using Workers.Servers;
    using static Interfaces.CommonConventions.Conventions;

    public class FlowUdpServer : IServer
    {
        private IPEndPoint EmptyEndPointInstance => new IPEndPoint(IPAddress.Any, 0);

        public void StartListeningToPort(int port)
        {
            new Thread(() =>
            {
                Console.Out.WriteLine("[ UDP ] SERVER IS RUNNING");

                var udpServer = new UdpClient(new IPEndPoint(IPAddress.Any, port));

                for (;;)
                {
                    IPEndPoint remoteClientEndPoint = this.EmptyEndPointInstance;

                    byte[] bufferByteArray = udpServer.Receive(ref remoteClientEndPoint);
                    Console.Out.WriteLine($"remoteClientEndPoint = {remoteClientEndPoint}");

                    string request = bufferByteArray.ToAsciiString();
                    Console.Out.WriteLine($"Remote Message: {request}");

                    if (request == QuitServerCmd)
                    {
                        bufferByteArray = "OK 200 [ UDP SERVER HALTED ]".ToAsciiEncodedByteArray();
                        udpServer.Send(bufferByteArray, bufferByteArray.Length, remoteClientEndPoint);
                        break;
                    }

                    UdpServerWorker.Instance
                        .Init(remoteClientEndPoint, udpServer)
                        .ExecuteRequest(request);
                }

                udpServer?.Close();
            }).Start();
        }
    }
}