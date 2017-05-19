namespace Presentation.Console.ClientApp {
    using System;
    using System.Net;
    using Protocol.Interfaces;

    public class UdpWorker : IWorker
    {
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