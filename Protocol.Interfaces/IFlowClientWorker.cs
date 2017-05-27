namespace FlowProtocol.Interfaces
{
    using System;
    using System.Net;
    using Common;
    using Response;

    /* TCP/UDP Client */
    public interface IFlowClientWorker : IFlowProtocol, IFlowProtocolResponseProcessor, IDisposable
    {
        void Init(IPAddress ipAddress, int port);
        void StartCommunication();
        string Authenticate(string login, string password);
    }
}