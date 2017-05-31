namespace FlowProtocol.Interfaces.Workers
{
    using System;
    using System.Net;
    using Common;

    /* TCP/UDP Client */
    public interface IFlowClientWorker : IFlowProtocol, IDisposable
    {
        //void Init(IPAddress ipAddress, int port);
        //void StartCommunication();
        //string Authenticate(string login, string password);
        //string Register(string login, string password, string name);

        bool Connect(IPAddress ipAddress, int port);
        bool Authenticate(string login, string password);
        bool Register(string login, string password, string name);
        string Translate(string sourceText, string sourceTextLang, string targetTextLanguage);
    }
}