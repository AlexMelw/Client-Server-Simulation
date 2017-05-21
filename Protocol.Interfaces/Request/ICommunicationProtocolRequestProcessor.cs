namespace Protocol.Interfaces.Request
{
    public interface ICommunicationProtocolRequestProcessor
    {
        void ProcessRequest(string response);

        //ConcurrentDictionary<string, string> ParseRequest(string request);
    }
}