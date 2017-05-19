namespace Protocol.Interfaces
{
    using System.Collections.Generic;

    public interface ICommunicationProtocolResponseProcessor
    {
        Dictionary<string, string> ParseResponse(string response);
        void ProcessResponse(Dictionary<string, string> parsedResponse);
    }
}