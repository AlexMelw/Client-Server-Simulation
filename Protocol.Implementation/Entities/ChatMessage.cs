namespace FlowProtocol.Implementation.Entities
{
    public class ChatMessage
    {
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string SourceLang { get; set; }
        public string TextBody { get; set; }
    }
}