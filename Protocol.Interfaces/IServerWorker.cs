namespace Protocol.Interfaces
{
    using System.Net;
    using Common;

    /* TCP/UDP Server */
    public interface IServerWorker : ICommunicationProtocol
    {
        void Init(IPAddress ipAddress, int port);
        void StartServing();
    }
}