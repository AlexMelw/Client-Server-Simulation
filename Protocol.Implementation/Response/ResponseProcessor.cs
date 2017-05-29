namespace FlowProtocol.Implementation.Response
{
    using System;
    using Interfaces.Response;

    public class ResponseProcessor : IFlowProtocolResponseProcessor
    {
        private readonly IFlowProtocolResponseParser _parser;

        #region CONSTRUCTORS

        public ResponseProcessor(IFlowProtocolResponseParser parser)
        {
            _parser = parser;
        }

        #endregion

        public string ProcessResponse(string response)
        {
            throw new NotImplementedException();
        }
    }
}