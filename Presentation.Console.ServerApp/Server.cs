namespace Presentation.Console.ServerApp
{
    using System;
    using FlowProtocol.Implementation.Servers;
    using static FlowProtocol.Interfaces.CommonConventions.Conventions;

    public class Server
    {
        private static void Main(string[] args)
        {
            FlowUdpServer.Instance.StartListeningToPort(UdpServerListeningPort);
            FlowTcpServer.Instance.StartListeningToPort(TcpServerListeningPort);
        }
    }
}