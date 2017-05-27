namespace FlowProtocol.Implementation.Workers.Clients
{
    using System;
    using System.Net;
    using Interfaces;
    using Interfaces.Response;

    public class UdpClientWorker : IFlowClientWorker
    {
        private readonly IFlowProtocolResponseParser _responseParser;

        #region CONSTRUCTORS

        public UdpClientWorker(IFlowProtocolResponseParser responseParser)
        {
            _responseParser = responseParser;
        }

        #endregion

        public void Send(string message)
        {
            throw new NotImplementedException();
        }

        public void Init(IPAddress ipAddress, int port)
        {
            throw new NotImplementedException();
        }

        public void StartCommunication()
        {
            throw new NotImplementedException();
        }

        string IFlowClientWorker.Authenticate(string login, string password)
        {
            throw new NotImplementedException();
        }

        public void Dispose() { }

        public byte[] ProcessResponseGetImageBytes(string response)
        {
            throw new NotImplementedException();
        }

        public bool IsAuthenticated(string response)
        {
            throw new NotImplementedException();
        }

        public void Register(string login, string password)
        {
            throw new NotImplementedException();
        }

        public void Authenticate(string login, string password)
        {
            throw new NotImplementedException();
        }

        public void Receive(string message)
        {
            throw new NotImplementedException();
        }
    }
}