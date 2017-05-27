namespace FlowProtocol.Interfaces.Common
{
    public interface IFlowProtocol
    {
        void Send(string message);
    }

    //Dictionary<string, string> ParseMessage(string message);
    //void ProcessMessage(Dictionary<string, string> parsedMessage);
}