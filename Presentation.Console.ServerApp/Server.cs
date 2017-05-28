namespace Presentation.Console.ServerApp
{
    using FlowProtocol.Implementation.Servers;
    using static FlowProtocol.Interfaces.CommonConventions.Conventions;

    public class Server
    {
        //static Server() => IoC.RegisterAll();

        private static void Main(string[] args)
        {
            //var flowServer = IoC.Resolve<IServer>();

            var flowUdpServer = new FlowUdpServer();
            var flowTcpServer = new FlowTcpServer();

            flowUdpServer.StartListeningToPort(UdpServerListeningPort);
            flowTcpServer.StartListeningToPort(TcpServerListeningPort);
        }
    }
}