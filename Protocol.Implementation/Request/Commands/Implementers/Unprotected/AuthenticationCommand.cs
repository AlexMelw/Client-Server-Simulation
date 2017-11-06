namespace FlowProtocol.Implementation.Request.Commands.Implementers.Unprotected
{
    using System;
    using System.Collections.Concurrent;
    using FlowProtocol.Interfaces.CommonConventions;
    using Interfaces;
    using Utilities;

    public class AuthenticationCommand : IRequestCommand, IFactoryRequestCommand
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
            _requestComponents.TryGetValue(Conventions.SessionKey, out string sessionKey);


            Guid authToken = CommandUtil.CreateNewSessionForUserWithCredentials(login, pass);

            if (authToken != Guid.Empty)
            {
                string originalMessage =
                    $@"200 OK AUTH --res='User authenticated successfully' --sessiontoken='{authToken}'";
                return CommandUtil.EncapsulateEncryptedMessage(originalMessage, sessionKey);
            }

            string originalMessage2 = $@"530 ERR AUTH --res='login or password incorrect'";

            return CommandUtil.EncapsulateEncryptedMessage(originalMessage2, sessionKey);
        }
    }
}