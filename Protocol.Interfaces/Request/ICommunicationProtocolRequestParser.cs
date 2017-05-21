namespace Protocol.Interfaces.Request
{
    using System.Collections.Concurrent;

    public interface ICommunicationProtocolRequestParser
    {
        ConcurrentDictionary<string, string> ParseResponse(string response);
    }
}