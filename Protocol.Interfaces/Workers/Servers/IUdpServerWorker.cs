namespace FlowProtocol.Interfaces.Workers.Servers
{
    using System.Net;
    using System.Net.Sockets;

    public interface IUdpServerWorker : IFlowServerWorker
    {
        void ExecuteRequest(string requestString);
        IUdpServerWorker Init(IPEndPoint remoteClientEndPoint, UdpClient udpServer);
    }
}