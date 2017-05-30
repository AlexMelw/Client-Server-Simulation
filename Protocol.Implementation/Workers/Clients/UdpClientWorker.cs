namespace FlowProtocol.Implementation.Workers.Clients
{
    using System.Net;
    using Interfaces.Response;
    using Interfaces.Workers;

    public class UdpClientWorker : IFlowClientWorker
    {
        private readonly IFlowProtocolResponseParser _parser;

        #region CONSTRUCTORS

        public UdpClientWorker(IFlowProtocolResponseParser parser)
        {
            _parser = parser;
        }

        #endregion

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public bool TryConnect(IPAddress ipAddress, int port)
        {
            throw new System.NotImplementedException();
        }

        public bool TryAuthenticate(string login, string password)
        {
            throw new System.NotImplementedException();
        }

        public bool TryRegister(string login, string password, string name)
        {
            throw new System.NotImplementedException();
        }

        public string Translate(string sourceText, string sourceTextLang, string targetTextLanguage)
        {
            throw new System.NotImplementedException();
        }
    }
}