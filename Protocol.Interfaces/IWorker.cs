namespace Protocol.Interfaces
{
    using System.Net;

    /* TCP/UDP Client */
    public interface IWorker : ICommunicationProtocol
    {
        void Init(IPAddress ipAddress, int port);
        void StartCommunication();
    }
}