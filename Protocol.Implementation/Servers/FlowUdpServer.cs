namespace FlowProtocol.Implementation.Servers
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using EasySharp.NHelpers;
    using Interfaces;
    using Interfaces.CommonConventions;
    using Workers.Servers;

    public class FlowUdpServer : IServer
    {
        public void StartListeningToPort(int port)
        {
            new Thread(() =>
            {
                Console.Out.WriteLine("[ UDP ] SERVER IS RUNNING");

                var udpServer = new UdpClient(new IPEndPoint(IPAddress.Any, port));

                for (;;)
                {
                    string request = string.Empty;
                    IPEndPoint remoteClientEndPoint = GetEmptyEndPoint();

                    byte[] bufferByteArray = udpServer.Receive(ref remoteClientEndPoint);
                    Console.Out.WriteLine($"remoteClientEndPoint = {remoteClientEndPoint}");

                    request = bufferByteArray.ToAsciiString();
                    Console.Out.WriteLine($"Remote Message: {request}");

                    if (request == Conventions.QuitServerCmd)
                    {
                        bufferByteArray = "OK 200 [ UDP SERVER HALTED ]".ToAsciiEncodedByteArray();
                        udpServer.Send(bufferByteArray, bufferByteArray.Length, remoteClientEndPoint);
                        break;
                    }

                    UdpServerWorker.Instance
                        .Init(remoteClientEndPoint, udpServer)
                        .ProcessRequest(request);
                }

                udpServer?.Close();
            }).Start();
        }

        private IPEndPoint GetEmptyEndPoint() => new IPEndPoint(IPAddress.Any, 0);
    }
}