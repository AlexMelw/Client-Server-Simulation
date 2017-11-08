namespace FlowProtocol.Implementation.Request.Commands.Implementers.Unprotected
{
    using System;
    using System.Collections.Concurrent;
    using DomainModels.Entities;
    using FlowProtocol.Interfaces.CommonConventions;
    using Interfaces;
    using Storage;
    using Utilities;

    public class GetMessageCommand : IRequestCommand, IRequestCommandFactory
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


            User user = CommandInterpreter.AuthenticateUser(sessionToken);

            if (user != null)
            {
                _requestComponents.TryGetValue(Conventions.TranslationMode, out string translationMode);

                if (translationMode == Conventions.DoNotTranslate)
                {
                    if (CorrespondenceManagement.Instance.ClientChatMessageQueues[user.Login]
                        .TryDequeue(out ChatMessage msg))
                    {
                        string originalMessage =
                            $"200 OK GETMSG --senderid='{msg.SenderId}' --sendername='{msg.SenderName}' --msg='{msg.TextBody}'";
                        return CommandInterpreter.EncapsulateEncryptedMessage(originalMessage, sessionKey);
                    }

                    string originalMessage2 = $@"513 ERR GETMSG --res='Message Box is empty'";
                    return CommandInterpreter.EncapsulateEncryptedMessage(originalMessage2, sessionKey);
                }

                if (translationMode == Conventions.DoTranslate)
                {
                    if (CorrespondenceManagement.Instance.ClientChatMessageQueues[user.Login]
                        .TryDequeue(out ChatMessage msg))
                    {
                        _requestComponents.TryGetValue(Conventions.TargetLang, out string targetLang);

                        string fromLang = msg.SourceLang == Conventions.Lang.Unknown ? "" : msg.SourceLang;
                        string toLang = targetLang == Conventions.Lang.Unknown
                            ? Conventions.Lang.English
                            : targetLang;

                        string translatedText;
                        try
                        {
                            translatedText = CommandInterpreter.Translate(
                                msg.TextBody,
                                fromLang,
                                toLang);
                        }
                        catch (Exception)
                        {
                            translatedText =
                                "[ Cognitive Services Reply: you have reached your translations limit for today ]";
                        }

                        string originalMessage =
                            $"200 OK GETMSG --senderid='{msg.SenderId}' --sendername='{msg.SenderName}' --msg='{translatedText}'";
                        return CommandInterpreter.EncapsulateEncryptedMessage(originalMessage, sessionKey);
                    }

                    string originalMessage2 = $@"513 ERR GETMSG --res='Message Box is empty'";
                    return CommandInterpreter.EncapsulateEncryptedMessage(originalMessage2, sessionKey);
                }
            }

            string originalMessage3 = $"511 ERR GETMSG --res='Athentication required'";
            return CommandInterpreter.EncapsulateEncryptedMessage(originalMessage3, sessionKey);
        }
    }
}