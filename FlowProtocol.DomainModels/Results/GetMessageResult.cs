namespace FlowProtocol.DomainModels.Results
{
    public class GetMessageResult
    {
        public bool Success { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string MessageBody { get; set; }
        public string ErrorExplained { get; set; }
    }
}