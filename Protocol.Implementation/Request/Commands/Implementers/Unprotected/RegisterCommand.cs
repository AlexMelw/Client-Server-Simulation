namespace FlowProtocol.Implementation.Request.Commands.Implementers.Unprotected
{
    using System.Collections.Concurrent;
    using FlowProtocol.Interfaces.CommonConventions;
    using Interfaces;
    using Utilities;

    public class RegisterCommand : IRequestCommand, IRequestCommandFactory
    {
        private ConcurrentDictionary<string, string> _requestComponents;

        public IRequestCommand BuildCommand(ConcurrentDictionary<string, string> requestComponents)
        {
            _requestComponents = requestComponents;

            return this;
        }

        public string Execute()
        {
            _requestComponents.TryGetValue(Conventions.Login, out string login);
            _requestComponents.TryGetValue(Conventions.Pass, out string pass);
            _requestComponents.TryGetValue(Conventions.Name, out string name);
            _requestComponents.TryGetValue(Conventions.SessionKey, out string sessionKey);

            if (CommandInterpreter.RegisterUser(login, pass, name))
            {
                string originalMessage = $@"200 OK REGISTER --res='User registered successfully'";
                string encapsulatedMessage = CommandInterpreter.EncapsulateEncryptedMessage(originalMessage, sessionKey);

                return encapsulatedMessage;
            }

            return $@"502 ERR REGISTER --res='User already exists'";
        }
    }
}