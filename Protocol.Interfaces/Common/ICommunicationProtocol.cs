namespace Protocol.Interfaces.Common
{
    public interface ICommunicationProtocol
    {
        void Send(string message);
    }

    //Dictionary<string, string> ParseMessage(string message);
    //void ProcessMessage(Dictionary<string, string> parsedMessage);
}