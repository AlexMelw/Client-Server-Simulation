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
            Kernel.Bind<ICommunicationProtocolRequestParser>()
                .To<RequestParser>();

            Kernel.Bind<ICommunicationProtocolResponseParser>()
                .To<ResponseParser>();

            // Register Protocol Processors
            Kernel.Bind<ICommunicationProtocolRequestProcessor>()
                .To<RequestProcessor>()
                .WithConstructorArgument(
                    name: "requestParser",
                    value: Kernel.Get<ICommunicationProtocolRequestParser>());

            Kernel.Bind<ICommunicationProtocolResponseProcessor>()
                .To<ResponseProcessor>()
                .WithConstructorArgument(
                    name: "responseParser",
                    value: Kernel.Get<ICommunicationProtocolResponseParser>());
        }

        public static T Resolve<T>()
        {
            return Kernel.Get<T>();
        }
    }
}