namespace Protocol.Interfaces.Response
{
    public interface ICommunicationProtocolResponseProcessor
    {
        byte[] ProcessResponseGetImageBytes(string response);

        //ConcurrentDictionary<string, string> ParseResponse(string response);
    }
}