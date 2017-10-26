namespace TestSerialization
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using FlowProtocol.DomainModels.Entities;
    using FlowProtocol.Implementation.Storage;

    class ProgramSerialization
    {
        static void Main(string[] args)
        {
            //XElement root = new XElement("ChatMessages");

            //foreach (KeyValuePair<string, ConcurrentQueue<ChatMessage>> pair
            //    in CorrespondenceManagement.Instance.ClientChatMessageQueues)
            //{
            //    root.Add(new XElement("MessageQueue",
            //        new XAttribute("UserLogin", pair.Key),
            //        pair.Value.Select(cm =>
            //            new XElement("ChatMessage",
            //                new XElement("SenderId", pair),
            //                new XElement("SenderName"),
            //                new XElement("SourceLang"),
            //                new XElement("TextBody")))));
            //}


        }
    }
}