namespace Presentation.Console.ServerApp
{
    using FlowProtocol.Interfaces.Servers;
    using Infrastructure;
    using static FlowProtocol.Interfaces.CommonConventions.Conventions;

    public class Server
    {
        private static void Main(string[] args)
        {
            IServer flowTcpServer = IoC.Resolve<IFlowTcpServer>();
            IServer flowUdpServer = IoC.Resolve<IFlowUdpServer>();

            flowTcpServer.StartListeningToPort(TcpServerListeningPort);
            flowUdpServer.StartListeningToPort(UdpServerListeningPort);
        }

        static Server() => IoC.RegisterAll();
    }
}