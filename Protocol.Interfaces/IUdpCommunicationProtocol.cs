namespace Protocol.Interfaces
{
    public interface IUdpCommunicationProtocol : ICommunicationProtocol
    {
        int ServingPort { get; }
    }
}
