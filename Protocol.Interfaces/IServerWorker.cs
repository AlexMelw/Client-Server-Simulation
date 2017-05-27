namespace Protocol.Interfaces
{
    using System;
    using System.Net;
    using Common;

    /* TCP/UDP Server */
    public interface IServerWorker : IFlowProtocol, IDisposable
    {
        void Init(IPAddress ipAddress, int port);
        void StartServing();
    }
}