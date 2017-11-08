namespace FlowProtocol.Implementation.Request.Commands.Implementers.Unprotected
{
    using System.Collections.Concurrent;
    using FlowProtocol.Interfaces.CommonConventions;
    using Interfaces;
    using Utilities;

    public class TranslateCommand : IRequestCommand, IRequestCommandFactory
    {
        private ConcurrentDictionary<string, string> _requestComponents;

        public IRequestCommand BuildCommand(ConcurrentDictionary<string, string> requestComponents)
        {
            _requestComponents = requestComponents;

            return this;
        }

        public string Execute()
        {
            _requestComponents.TryGetValue(Conventions.SourceText, out string sourceText);
            _requestComponents.TryGetValue(Conventions.SourceLang, out string sourceLang);
            _requestComponents.TryGetValue(Conventions.TargetLang, out string targetLang);
            _requestComponents.TryGetValue(Conventions.SessionKey, out string sessionKey);


            string translatedText = CommandInterpreter.Translate(
                sourceText,
                sourceLang,
                targetLang
            );

            string originalMessage = $@"200 OK TRANSLATE --res='{translatedText}'";

            return CommandInterpreter.EncapsulateEncryptedMessage(originalMessage, sessionKey);
        }
    }
}