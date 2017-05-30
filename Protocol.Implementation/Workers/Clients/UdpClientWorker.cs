namespace FlowProtocol.Implementation.Workers.Clients
{
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
    }
}