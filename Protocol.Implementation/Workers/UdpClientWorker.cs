namespace Protocol.Implementation.Workers
{
    using System;
    using System.Net;
    using Interfaces;
    using Interfaces.Response;

    public class UdpClientWorker : IClientWorker
    {
        private IFlowProtocolResponseProcessor _responseProcessor;

        #region CONSTRUCTORS

        public UdpClientWorker(IFlowProtocolResponseProcessor responseProcessor)
        {
            _responseProcessor = responseProcessor;
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

        public void Dispose() { }
    }
}