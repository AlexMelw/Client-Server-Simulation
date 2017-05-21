namespace Protocol.Interfaces.Response
{
    using System.Collections.Concurrent;

    public interface IFlowProtocolResponseParser
    {
        ConcurrentDictionary<string, string> ParseResponse(string response);
    }
}