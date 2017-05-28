namespace FlowProtocol.Implementation.Storage
{
    using System;
    using System.Collections.Concurrent;
    using Entities;

    public sealed class CorrespondenceManagement
    {
        private static readonly Lazy<CorrespondenceManagement> Lazy =
            new Lazy<CorrespondenceManagement>(() => new CorrespondenceManagement(), true);

        public readonly ConcurrentDictionary<string, ConcurrentQueue<ChatMessage>> ClientChatMessageQueues;

        public static CorrespondenceManagement Instance => Lazy.Value;

        #region CONSTRUCTORS

        private CorrespondenceManagement()
        {
            ClientChatMessageQueues = new ConcurrentDictionary<string, ConcurrentQueue<ChatMessage>>();
        }

        #endregion
    }
}