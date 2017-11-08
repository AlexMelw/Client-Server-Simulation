namespace FlowProtocol.Implementation.Request.Commands.Interfaces
{
    using System.Collections.Concurrent;

    public interface IRequestCommandFactory
    {
        IRequestCommand BuildCommand(ConcurrentDictionary<string, string> requestComponents);
    }
}