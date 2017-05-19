namespace Protocol.Interfaces
{
    using System.Net;

    /* TCP/UDP Server */
    public interface IServerWorker : ICommunicationProtocol
    {
        void Init(IPAddress ipAddress, int port);
        void StartServing();
    }
}