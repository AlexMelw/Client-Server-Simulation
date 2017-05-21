namespace Protocol.Interfaces
{
    using System.Net;
    using Common;

    /* TCP/UDP Client */
    public interface IClientWorker : ICommunicationProtocol
    {
        void Init(IPAddress ipAddress, int port);
        void StartCommunication();
    }
}