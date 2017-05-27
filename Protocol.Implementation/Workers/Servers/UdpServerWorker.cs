namespace FlowProtocol.Implementation.Workers.Servers
{
    using System.Net;
    using Interfaces;
    internal class UdpServerWorker: IFlowServerWorker {
        public void Send(string message)
        {
            throw new System.NotImplementedException();
        }

        public void ProcessRequest(string response)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void Init(IPAddress ipAddress, int port)
        {
            throw new System.NotImplementedException();
        }

        public void StartServing()
        {
            throw new System.NotImplementedException();
        }
    }
}