namespace Protocol.Implementation
{
    using System;
    using System.Collections.Generic;
    using Interfaces;

    public class RequestProcessor : ICommunicationProtocolRequestProcessor
    {
        public Dictionary<string, string> ParseRequest(string request)
        {
            throw new NotImplementedException();
        }

        public void ProcessRequest(Dictionary<string, string> parsedRequest)
        {
            throw new NotImplementedException();
        }
    }
}