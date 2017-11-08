namespace FlowProtocol.Implementation.Request
{
    using Commands;
    using Commands.Interfaces;
    using Interfaces.Request;
    using Ninject;

    public class RequestProcessor : IFlowProtocolRequestProcessor
    {
        //private const string AppId = "6CE9C85A41571C050C379F60DA173D286384E0F2";

        private readonly IFlowProtocolRequestParser _parser;

        #region CONSTRUCTORS

        [Inject]
        public RequestProcessor(IFlowProtocolRequestParser parser)
        {
            _parser = parser;
        }

        #endregion

        public string ProcessRequest(string request)
        {
            var commandsContainer = new CommandsContainer();

            var commandProcessor = new RequestCommandFactory(commandsContainer.Commands, _parser);

            IRequestCommand requestCommand = commandProcessor.CreateProtectedRequestCommand(request);

            return requestCommand.Execute();
        }
    }
}