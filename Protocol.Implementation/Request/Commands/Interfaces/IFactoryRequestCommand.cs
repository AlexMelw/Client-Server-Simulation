namespace FlowProtocol.Implementation.Request.Commands.Interfaces
{
    using System.Collections.Concurrent;

    public interface IFactoryRequestCommand
    {
        IRequestCommand BuildCommand(ConcurrentDictionary<string, string> requestComponents);
    }
}