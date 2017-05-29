namespace FlowProtocol.Interfaces
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using Common;
    using Request;

    /* TCP/UDP Server */
    public interface IFlowServerWorker : IFlowProtocol
    {
        void ExecuteRequest(string request);
    }
}