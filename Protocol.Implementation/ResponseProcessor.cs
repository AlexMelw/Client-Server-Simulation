namespace Protocol.Implementation
{
    using System;
    using System.Collections.Generic;
    using Interfaces;

    public class ResponseProcessor : ICommunicationProtocolResponseProcessor
    {
        public Dictionary<string, string> ParseResponse(string response)
        {
            throw new NotImplementedException();
        }

        public void ProcessResponse(Dictionary<string, string> parsedResponse)
        {
            throw new NotImplementedException();
        }
    }
}