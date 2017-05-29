namespace FlowProtocol.Implementation.Workers.Clients
{
    using System;
    using System.Net;
    using Interfaces;
    using Interfaces.Response;
    using Interfaces.Workers;

    public class UdpClientWorker : IFlowClientWorker
    {
        private readonly IFlowProtocolResponseProcessor _responseProcessor;
        public UdpClientWorker(IFlowProtocolResponseProcessor responseProcessor)
        {
            _responseProcessor = responseProcessor;
        }
    }
}