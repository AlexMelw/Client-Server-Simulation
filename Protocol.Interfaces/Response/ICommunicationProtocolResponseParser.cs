namespace Protocol.Interfaces.Response
{
    using System.Collections.Concurrent;

    public interface ICommunicationProtocolResponseParser
    {
        ConcurrentDictionary<string, string> ParseResponse(string response);
    }
}