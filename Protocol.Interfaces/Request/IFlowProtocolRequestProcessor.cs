namespace FlowProtocol.Interfaces.Request
{
    public interface IFlowProtocolRequestProcessor
    {
        void ProcessRequest(string response);

        //ConcurrentDictionary<string, string> ParseRequest(string request);
    }
}