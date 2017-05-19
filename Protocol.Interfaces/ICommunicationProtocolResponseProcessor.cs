namespace Protocol.Interfaces
{
    public interface ICommunicationProtocolResponseProcessor
    {
        string ProcessResponseGetImageSrc(string response);

        //ConcurrentDictionary<string, string> ParseResponse(string response);
    }
}