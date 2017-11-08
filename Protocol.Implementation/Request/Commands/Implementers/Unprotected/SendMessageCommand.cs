namespace FlowProtocol.Implementation.Request.Commands.Implementers.Unprotected
{
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using DomainModels.Entities;
    using FlowProtocol.Interfaces.CommonConventions;
    using Interfaces;
    using Storage;
    using Utilities;

    public class SendMessageCommand : IRequestCommand, IRequestCommandFactory
    {
        private ConcurrentDictionary<string, string> _requestComponents;

        public IRequestCommand BuildCommand(ConcurrentDictionary<string, string> requestComponents)
        {
            _requestComponents = requestComponents;

            return this;
        }

        public string Execute()
        {
            _requestComponents.TryGetValue(Conventions.SessionToken, out string sessionToken);
            _requestComponents.TryGetValue(Conventions.SessionKey, out string sessionKey);

            User senderUser = CommandInterpreter.AuthenticateUser(sessionToken);

            if (senderUser != null)
            {
                _requestComponents.TryGetValue(Conventions.Recipient, out string recipient);

                if (CommandInterpreter.AuthenticateRecipient(recipient))
                {
                    _requestComponents.TryGetValue(Conventions.Message, out string message);
                    _requestComponents.TryGetValue(Conventions.SourceLang, out string sourceLang);

                    Debug.Assert(recipient != null, "recipient != null");
                    CorrespondenceManagement.Instance.ClientChatMessageQueues[recipient]
                        .Enqueue(new ChatMessage
                        {
                            SourceLang = sourceLang,
                            TextBody = message,
                            SenderId = senderUser.Login,
                            SenderName = senderUser.Name
                        });

                    string originalMessage = $@"200 OK SENDMSG --res='Message sent successfully'";
                    return CommandInterpreter.EncapsulateEncryptedMessage(originalMessage, sessionKey);
                }

                string originalMessage2 = $@"512 ERR SENDMSG --res='Inexistent recipient'";
                return CommandInterpreter.EncapsulateEncryptedMessage(originalMessage2, sessionKey);
            }

            string originalMessage3 = $@"511 ERR SENDMSG --res='Athentication required'";
            return CommandInterpreter.EncapsulateEncryptedMessage(originalMessage3, sessionKey);
        }
    }
}