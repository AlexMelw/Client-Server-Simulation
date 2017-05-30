namespace FlowProtocol.Interfaces.Workers
{
    using System.Net;
    using Common;

    /* TCP/UDP Client */
    public interface IFlowClientWorker : IFlowProtocol
    {
        //void Init(IPAddress ipAddress, int port);
        //void StartCommunication();
        //string Authenticate(string login, string password);
        //string Register(string login, string password, string name);

        bool TryConnect(IPAddress ipAddress, int port);
        bool TryAuthenticate(string login, string password);
        bool TryRegister(string login, string password, string name);
    }
}