namespace Presentation.Console.ClientApp {
    using System;
    using System.Collections.Generic;
    using Protocol.Interfaces;

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