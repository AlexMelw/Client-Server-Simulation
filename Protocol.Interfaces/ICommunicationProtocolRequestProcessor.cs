namespace Protocol.Interfaces
{
    public interface ICommunicationProtocolRequestProcessor
    {
        void ProcessRequest(string response);

        //ConcurrentDictionary<string, string> ParseRequest(string request);
    }
}