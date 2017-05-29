namespace FlowProtocol.Implementation.Workers.Servers
{
    public interface IFlowProtocolRequestProcessor
    {
        string ProcessRequest(string request);
    }
}