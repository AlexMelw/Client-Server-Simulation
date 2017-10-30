namespace Presentation.Console.ServerApp
{
    using FlowProtocol.Interfaces.Servers;
    using Infrastructure;
    using static FlowProtocol.Interfaces.CommonConventions.Conventions;

    public class Server
    {
        #region CONSTRUCTORS

        static Server() => IoC.RegisterAll();

        #endregion

        private static void Main(string[] args)
        {
            System.Console.Title = "Flow Server is running";

            IServer flowTcpServer = IoC.Resolve<IFlowTcpServer>();
            IServer flowUdpServer = IoC.Resolve<IFlowUdpServer>();

            flowTcpServer.StartListeningToPort(TcpServerListeningPort);
            flowUdpServer.StartListeningToPort(UdpServerListeningPort);
        }
    }
}