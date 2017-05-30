namespace FlowProtocol.Implementation.Servers
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using EasySharp.NHelpers.Wrappers;
    using Workers.Servers;
    using static Interfaces.CommonConventions.Conventions;

    public class FlowTcpServer
    {
        public static FlowTcpServer Instance => new FlowTcpServer();

        #region CONSTRUCTORS

        private FlowTcpServer() { }

        #endregion

        public void StartListeningToPort(int port)
        {
            new Thread(() =>
            {
                var ipAddress = IPAddress.Parse(Localhost);
                var tcpListener = new TcpListenerEx(ipAddress, TcpServerListeningPort);

                try
                {
                    tcpListener.Start();

                    Console.WriteLine(" The local End point is  :" + tcpListener.LocalEndpoint);
                    Console.WriteLine(" Waiting for a connection.....");
                    Console.Out.WriteLine();

                    while (true) // is serving continuously
                    {
                        Socket workerTcpSocket = tcpListener.AcceptSocket();

                        Console.WriteLine($" Connection accepted from: {{ {workerTcpSocket.RemoteEndPoint} }}");
                        Console.WriteLine($" SoketWorker is bound to: {{ {workerTcpSocket.LocalEndPoint} }}");

                        TcpServerWorker.Instance
                            .Init(workerTcpSocket, tcpListener)
                            .StartWorking();
                    }
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("Grave error occured. Searver is dead.");
                    Console.Out.WriteLine($"e = {e.Message}");
                    Debug.WriteLine("Grave error occured. Searver is dead.");
                    Debug.WriteLine($"e = {e.Message}");
                    Console.Out.WriteLine("PRESS ANY KEY TO QUIT");
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