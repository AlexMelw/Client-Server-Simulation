namespace FlowProtocol.Implementation.Request.Commands.Implementers.Protected
{
    using System;
    using System.Collections.Concurrent;
    using FlowProtocol.Interfaces.CommonConventions;
    using FlowProtocol.Interfaces.Request;
    using Interfaces;
    using Utilities;

    public class ConfidentialCommand : IRequestCommand, IFactoryRequestCommand
    {
        private ConcurrentDictionary<string, string> _requestComponents;

        private readonly IFlowProtocolRequestParser _parser;

        #region CONSTRUCTORS

        public ConfidentialCommand(IFlowProtocolRequestParser parser) => _parser = parser;

        public ConfidentialCommand() : this(new RequestParser()) { }

        #endregion

        public IRequestCommand BuildCommand(ConcurrentDictionary<string, string> requestComponents)
        {
            _requestComponents = requestComponents;

            return this;
        }

        public string Execute()
        {
            #region Personal Responsibility

            _requestComponents.TryGetValue(Conventions.SessionKey, out string sessionKey);
            _requestComponents.TryGetValue(Conventions.Secret, out string secret);

            string decryptedRequestMessage = CommandUtil.DecryptSecret(secret, sessionKey);

            var requestComponents = _parser.ParseRequest(decryptedRequestMessage);

            requestComponents.TryGetValue(Conventions.Cmd, out string innerCmd);

            #endregion

            #region Delegated Responsibility

            try
            {
                var commandsContainer = new CommandsContainer();

                var commandProcessor = new RequestCommandProcessor(commandsContainer.Commands, _parser);

                IRequestCommand requestCommand =
                    commandProcessor.CreateUnprotectedRequestCommand(decryptedRequestMessage, sessionKey);

                return requestCommand.Execute();
            }
            catch (Exception)
            {
                return Conventions.BadRequest;
            }

            #endregion
        }
    }
}