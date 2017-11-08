namespace FlowProtocol.Implementation.Request.Commands
{
    using System;
    using System.Collections.Concurrent;
    using FlowProtocol.Interfaces.CommonConventions;
    using FlowProtocol.Interfaces.Request;
    using Implementers.Unprotected;
    using Interfaces;

    public class RequestCommandFactory
    {
        private readonly ConcurrentDictionary<string, Lazy<IRequestCommandFactory>> _requestProcessingCommands;
        private readonly IFlowProtocolRequestParser _parser;

        #region CONSTRUCTORS

        public RequestCommandFactory(
            ConcurrentDictionary<string, Lazy<IRequestCommandFactory>> requestProcessingCommands,
            IFlowProtocolRequestParser parser)
        {
            _requestProcessingCommands = requestProcessingCommands
                                         ?? throw new ArgumentNullException(nameof(requestProcessingCommands));

            _parser = parser
                      ?? throw new ArgumentNullException(nameof(parser));
        }

        #endregion

        public IRequestCommand CreateProtectedRequestCommand(string encryptedRequestMessage)
        {
            var requestComponents = _parser.ParseRequest(encryptedRequestMessage);

            if (requestComponents.TryGetValue(Conventions.Cmd, out string cmd))
            {
                if (_requestProcessingCommands.TryGetValue(cmd, out Lazy<IRequestCommandFactory> lazyCommandFactory))
                {
                    return lazyCommandFactory.Value.BuildCommand(requestComponents);
                }
            }

            return new NotFoundRequestCommand(cmd);
        }

        public IRequestCommand CreateUnprotectedRequestCommand(string decryptedRequestMessage, string sessionKey)
        {
            var requestComponents = _parser.ParseRequest(decryptedRequestMessage);
            requestComponents.TryAdd(Conventions.SessionKey, sessionKey);

            if (requestComponents.TryGetValue(Conventions.Cmd, out string cmd))
            {
                if (_requestProcessingCommands.TryGetValue(cmd, out Lazy<IRequestCommandFactory> lazyCommandFactory))
                {
                    return lazyCommandFactory.Value.BuildCommand(requestComponents);
                }
            }

            // Null object pattern
            return new NotFoundRequestCommand(cmd);
        }
    }
}