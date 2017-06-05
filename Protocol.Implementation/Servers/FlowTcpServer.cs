namespace FlowProtocol.Implementation.Servers
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using EasySharp.NHelpers.Wrappers;
    using Interfaces.Servers;
    using Workers.Servers;
    using static Interfaces.CommonConventions.Conventions;

    public class FlowTcpServer : IFlowTcpServer
    {
        public static FlowTcpServer Instance => new FlowTcpServer();

        #region CONSTRUCTORS

        public FlowTcpServer() { }

        #endregion

        public void StartListeningToPort(int port)
        {
            new Thread(() =>
            {
                Console.Out.WriteLine(" [TCP] SERVER IS RUNNING");

                var ipAddress = IPAddress.Parse(Localhost);
                var tcpListener = new TcpListenerEx(ipAddress, TcpServerListeningPort);

                try
                {
                    tcpListener.Start();

                    Console.WriteLine(" [TCP] The local End point is  :" + tcpListener.LocalEndpoint);
                    Console.WriteLine(" [TCP] Waiting for a connection.....");
                    Console.Out.WriteLine();

                    while (true) // is serving continuously
                    {
                        Socket workerTcpSocket = tcpListener.AcceptSocket();

                        Console.WriteLine($" [TCP] Connection accepted from: {{ {workerTcpSocket.RemoteEndPoint} }}");
                        Console.WriteLine($" [TCP] SoketWorker is bound to: {{ {workerTcpSocket.LocalEndPoint} }}");

                        TcpServerWorker.Instance
                            .Init(workerTcpSocket, tcpListener)
                            .StartWorking();
                    }
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("[TCP] Grave error occured. Searver is dead.");
                    Console.Out.WriteLine($"e = {e.Message}");
                    Debug.WriteLine("[TCP] Grave error occured. Searver is dead.");
                    Debug.WriteLine($"e = {e.Message}");
                    Console.Out.WriteLine("[TCP] PRESS ANY KEY TO QUIT");
                    Console.ReadLine();
                    throw;
                }
                finally
                {
                    if (tcpListener.Active)
                    {
                        tcpListener.Stop();
                    }
                }
            }).Start();
        }
    }
}