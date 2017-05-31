namespace FlowProtocol.Implementation.Storage
{
    using System;
    using System.Collections.Concurrent;
    using DomainModels.Entities;

    public sealed class CorrespondenceManagement
    {
        private static readonly Lazy<CorrespondenceManagement> Lazy =
            new Lazy<CorrespondenceManagement>(() => new CorrespondenceManagement(), true);

        public readonly ConcurrentDictionary<string, ConcurrentQueue<ChatMessage>> ClientChatMessageQueues;

        public static CorrespondenceManagement Instance => Lazy.Value;

        public bool TryCreateMailboxForUser(User user)
        {
            if (ClientChatMessageQueues.ContainsKey(user.Login))
            {
                return false;
            }
            return ClientChatMessageQueues.TryAdd(user.Login, new ConcurrentQueue<ChatMessage>());
        }

        #region CONSTRUCTORS

        private CorrespondenceManagement()
        {
            ClientChatMessageQueues = new ConcurrentDictionary<string, ConcurrentQueue<ChatMessage>>();
        }

        #endregion
    }
}