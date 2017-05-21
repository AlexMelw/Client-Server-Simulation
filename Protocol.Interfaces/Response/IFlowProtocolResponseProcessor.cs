namespace Protocol.Interfaces.Response
{
    public interface IFlowProtocolResponseProcessor
    {
        byte[] ProcessResponseGetImageBytes(string response);

        //ConcurrentDictionary<string, string> ParseResponse(string response);
    }
}