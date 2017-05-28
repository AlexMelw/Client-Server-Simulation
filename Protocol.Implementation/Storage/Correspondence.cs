namespace FlowProtocol.Implementation.Storage
{
    using System;
    using System.Collections.Concurrent;
    using Entities;

    public sealed class Correspondence
    {
        private static readonly Lazy<Correspondence> Lazy =
            new Lazy<Correspondence>(() => new Correspondence(), true);

        public readonly ConcurrentDictionary<User, ConcurrentQueue<ChatMessage>> ClientChatMessageQueues;

        public static Correspondence Instance => Lazy.Value;

        #region CONSTRUCTORS

        private Correspondence()
        {
            ClientChatMessageQueues = new ConcurrentDictionary<User, ConcurrentQueue<ChatMessage>>();
        }

        #endregion
    }
}