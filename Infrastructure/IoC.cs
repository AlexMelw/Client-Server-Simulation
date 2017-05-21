namespace Infrastructure
{
    using Ninject;
    using Protocol.Implementation.Request;
    using Protocol.Implementation.Response;
    using Protocol.Interfaces.Request;
    using Protocol.Interfaces.Response;

    public class IoC
    {
        private static readonly IKernel Kernel = new StandardKernel();

        public static void RegisterAll()
        {
            // Register Protocol Parsers
            Kernel.Bind<IFlowProtocolRequestParser>()
                .To<RequestParser>();

            Kernel.Bind<IFlowProtocolResponseParser>()
                .To<ResponseParser>();

            // Register Protocol Processors
            Kernel.Bind<IFlowProtocolRequestProcessor>()
                .To<RequestProcessor>()
                .WithConstructorArgument(
                    name: "requestParser",
                    value: Kernel.Get<IFlowProtocolRequestParser>());

            Kernel.Bind<IFlowProtocolResponseProcessor>()
                .To<ResponseProcessor>()
                .WithConstructorArgument(
                    name: "responseParser",
                    value: Kernel.Get<IFlowProtocolResponseParser>());
        }

        public static T Resolve<T>()
        {
            return Kernel.Get<T>();
        }
    }
}