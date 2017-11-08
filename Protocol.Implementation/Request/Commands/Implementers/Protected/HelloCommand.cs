namespace FlowProtocol.Implementation.Request.Commands.Implementers.Protected
{
    using System;
    using System.Collections.Concurrent;
    using FlowProtocol.Interfaces.CommonConventions;
    using Interfaces;
    using ProtocolHelpers;
    using Storage;
    using Utilities;

    public class HelloCommand : IRequestCommand, IRequestCommandFactory
    {
        private ConcurrentDictionary<string, string> _requestComponents;

        public IRequestCommand BuildCommand(ConcurrentDictionary<string, string> requestComponents)
        {
            _requestComponents = requestComponents;

            return this;
        }

        public string Execute()
        {
            //return $@"200 OK HELLO";
            _requestComponents.TryGetValue(Conventions.Exponent, out string clientEncryptionExponent);
            _requestComponents.TryGetValue(Conventions.Modulus, out string clientEncryptionModulus);

            Guid sessionKey = CommandInterpreter.CreateSessionKey(clientEncryptionExponent, clientEncryptionModulus);

            SecureSessionMap.Instance.Keeper.TryGetValue(sessionKey, out var keys);

            string e = keys.ServerPublicKey.Exponent.ToBase64String();
            string m = keys.ServerPublicKey.Modulus.ToBase64String();

            // Must not be encrypted !
            return $"200 OK HELLO --pubkey='{e}|{m}' --sessionkey='{sessionKey}'";

            //return $@"502 ERR HELLO --res='Cannot establish a secure connection'";
        }
    }
}