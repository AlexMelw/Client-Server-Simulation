namespace FlowProtocol.Implementation.Servers
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using Interfaces.Servers;
    using ProtocolHelpers;
    using Workers.Servers;
    using static Interfaces.CommonConventions.Conventions;

    public class FlowUdpServer : IFlowUdpServer
    {
        private IPEndPoint EmptyEndPointInstance => new IPEndPoint(IPAddress.Any, 0);
        public static FlowUdpServer Instance => new FlowUdpServer();

        #region CONSTRUCTORS

        public FlowUdpServer() { }

        #endregion

        public void StartListeningToPort(int port)
        {
            new Thread(() =>
            {
                Console.Out.WriteLine(" [UDP] SERVER IS RUNNING");
                
                using (var udpServer = new UdpClient(new IPEndPoint(IPAddress.Any, port)))
                {
                    bool isServingRequests = true;

                    while(isServingRequests)
                    {
                        IPEndPoint remoteClientEndPoint = EmptyEndPointInstance;

                        byte[] bufferByteArray = udpServer.Receive(ref remoteClientEndPoint);
                        Console.Out.WriteLine($" [UDP] remoteClientEndPoint = {remoteClientEndPoint}");

                        string request = bufferByteArray.ToFlowProtocolAsciiDecodedString();
                        Console.Out.WriteLine($" [UDP] Remote Message: {request}");

                        if (request == QuitServerCmd)
                        {
                            bufferByteArray = "200 OK SHUTDOWN --res='UDP Server Halted'".ToFlowProtocolAsciiEncodedBytesArray();
                            udpServer.Send(bufferByteArray, bufferByteArray.Length, remoteClientEndPoint);
                            isServingRequests = false;
                            continue;
                        }

                        UdpServerWorker.Instance
                            .Init(remoteClientEndPoint, udpServer)
                            .ExecuteRequest(request);
                    }
                }
            }).Start();
        }
    }
}