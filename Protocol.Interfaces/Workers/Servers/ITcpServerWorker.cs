namespace FlowProtocol.Interfaces.Workers.Servers
{
    using System.Net.Sockets;

    public interface ITcpServerWorker : IFlowServerWorker
    {
        ITcpServerWorker Init(Socket workerTcpSocket, TcpListenerEx tcpListener);
        void StartWorking();
    }
}