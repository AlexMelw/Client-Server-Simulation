namespace Protocol.Interfaces
{
    using System;
    using System.Net;
    using Common;

    /* TCP/UDP Client */
    public interface IClientWorker : IFlowProtocol, IDisposable
    {
        void Init(IPAddress ipAddress, int port);
        void StartCommunication();
        string Authenticate(string login, string password);
    }
}