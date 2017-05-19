namespace Protocol.Interfaces
{
    using System.Collections.Generic;

    public interface ICommunicationProtocolRequestProcessor
    {
        Dictionary<string, string> ParseRequest(string request);
        void ProcessRequest(Dictionary<string, string> parsedRequest);
    }
}