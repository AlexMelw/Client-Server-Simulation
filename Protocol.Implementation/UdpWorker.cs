namespace Protocol.Implementation
{
    using System;
    using System.Net;
    using Protocol.Interfaces;

    public class UdpWorker : IWorker
    {
        private ICommunicationProtocolResponseProcessor _responseProcessor;
        public UdpWorker(ICommunicationProtocolResponseProcessor responseProcessor)
        {
            _responseProcessor = responseProcessor;
        }

        public void Send(string message)
        {
            throw new NotImplementedException();
        }

        public void Receive(string message)
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
    }
}