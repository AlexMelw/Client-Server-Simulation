namespace Protocol.Interfaces
{
    public interface ITcpCommunicationProtocol : ICommunicationProtocol
    {
        int ServingPort { get; }
    }
}