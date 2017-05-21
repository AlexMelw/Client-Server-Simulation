namespace Protocol.Implementation.Workers
{
    using System;
    using System.Net;
    using Interfaces;
    using Interfaces.Response;

    public class UdpClientWorker : IClientWorker
    {
        private ICommunicationProtocolResponseProcessor _responseProcessor;

        #region CONSTRUCTORS

        public UdpClientWorker(ICommunicationProtocolResponseProcessor responseProcessor)
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

        public void Receive(string message)
        {
            throw new NotImplementedException();
        }
    }
}