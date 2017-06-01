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

            FlowUdpServer.Instance.StartListeningToPort(UdpServerListeningPort);
            FlowTcpServer.Instance.StartListeningToPort(TcpServerListeningPort);
        }
    }
}