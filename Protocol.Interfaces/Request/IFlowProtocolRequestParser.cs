namespace Protocol.Interfaces.Request
{
    using System.Collections.Concurrent;

    public interface IFlowProtocolRequestParser
    {
        ConcurrentDictionary<string, string> ParseResponse(string response);
    }
}