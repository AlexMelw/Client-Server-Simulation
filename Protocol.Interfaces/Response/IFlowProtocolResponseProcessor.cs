namespace Protocol.Interfaces.Response
{
    public interface IFlowProtocolResponseProcessor
    {
        byte[] ProcessResponseGetImageBytes(string response);

        bool IsAuthenticated(string response);
    }
}