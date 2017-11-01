namespace FlowProtocol.Interfaces.Workers.Servers
{
    using System.Net.Sockets;
    using EasySharp.NHelpers.CustomWrappers.Networking;

    public interface ITcpServerWorker : IFlowServerWorker
    {
        ITcpServerWorker Init(Socket workerTcpSocket, TcpListenerEx tcpListener);
        void StartWorking();
    }
}