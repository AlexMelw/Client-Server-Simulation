namespace FlowProtocol.Interfaces
{
    using System;
    using System.Net;
    using Common;
    using Request;

    /* TCP/UDP Server */
    public interface IFlowServerWorker : IFlowProtocol, IFlowProtocolRequestProcessor, IDisposable
    {
        void Init(IPAddress ipAddress, int port);
        void StartServing();
    }
}