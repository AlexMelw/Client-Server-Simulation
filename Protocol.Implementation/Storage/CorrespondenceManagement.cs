namespace FlowProtocol.Implementation.Storage
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using DomainModels.Entities;

    public sealed class CorrespondenceManagement
    {
        private static readonly object PadLock = new object();

        private static readonly Lazy<CorrespondenceManagement> Lazy =
            new Lazy<CorrespondenceManagement>(() => new CorrespondenceManagement(), true);

        public readonly ConcurrentDictionary<string, ConcurrentQueue<ChatMessage>> ClientChatMessageQueues;
        private static readonly string XmlFile = "CorrespondenceManagement.xml";

        public static CorrespondenceManagement Instance => Lazy.Value;

        public void UpdateLocalStorage()
        {
            lock (PadLock)
            {
                File.Delete(XmlFile);

                XElement root = new XElement("CorrespondenceManagement");

                foreach (KeyValuePair<string, ConcurrentQueue<ChatMessage>> pair
                    in CorrespondenceManagement.Instance.ClientChatMessageQueues)
                {
                    root.Add(new XElement("MessageQueue",
                        new XAttribute("UserLogin", pair.Key),
                        pair.Value.Select(chatMessage =>
                            new XElement("ChatMessage",
                                new XElement("SenderId", chatMessage.SenderId),
                                new XElement("SenderName", chatMessage.SenderName),
                                new XElement("SourceLang", chatMessage.SourceLang),
                                new XElement("TextBody", chatMessage.TextBody)))));
                }

                root.Save(XmlFile, SaveOptions.None);
            }
        }

        #region CONSTRUCTORS

        private CorrespondenceManagement()
        {
            ClientChatMessageQueues = new ConcurrentDictionary<string, ConcurrentQueue<ChatMessage>>();

            if (!File.Exists(XmlFile))
            {
                XElement root = new XElement("ChatMessages");
                root.Save("CorrespondenceManagement.xml", SaveOptions.None);
            }
            else
            {
                XElement root = XElement.Load(XmlFile, LoadOptions.PreserveWhitespace);
                foreach (XElement node in root.Elements("MessageQueue"))
                {
                    string userLogin = node.Attribute("UserLogin").Value;

                    if (RegisteredUsers.Instance.Users.TryGetValue(userLogin, out User user))
                    {
                        TryCreateMailboxForUser(user);

                        ConcurrentQueue<ChatMessage> userQueue = ClientChatMessageQueues[user.Login];

                        List<ChatMessage> chatMessages = node.Elements("ChatMessage").Select(xChatMessage => new ChatMessage
                        {
                            SenderId = xChatMessage.Element("SenderId").Value,
                            SenderName = xChatMessage.Element("SenderName").Value,
                            SourceLang = xChatMessage.Element("SourceLang").Value,
                            TextBody = xChatMessage.Element("TextBody").Value
                        }).ToList();

                        chatMessages.ForEach(chatMessage => userQueue.Enqueue(chatMessage));
                    }
                }
            }
        }

        #endregion

        public bool TryCreateMailboxForUser(User user)
        {
            if (ClientChatMessageQueues.ContainsKey(user.Login))
            {
                return false;
            }

            bool success = ClientChatMessageQueues.TryAdd(user.Login, new ConcurrentQueue<ChatMessage>());

            if (success)
            {
                lock (PadLock)
                {
                    XElement root = XElement.Load(XmlFile, LoadOptions.PreserveWhitespace);

                    XElement userQueue = root
                        .Elements("MessageQueue")
                        .FirstOrDefault(queue => queue.Attribute("UserLogin").Value == user.Login);

                    if (userQueue == null)
                    {
                        root.Add(new XElement("MessageQueue",
                            new XAttribute("UserLogin", user.Login)));

                        root.Save(XmlFile, SaveOptions.None);
                    }
                }
            }

            return success;
        }
    }
}