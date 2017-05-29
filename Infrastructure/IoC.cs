namespace Infrastructure
{
    using FlowProtocol.Implementation.Request;
    using FlowProtocol.Implementation.Servers;
    using FlowProtocol.Implementation.Workers.Servers;
    using FlowProtocol.Interfaces;
    using FlowProtocol.Interfaces.Request;
    using FlowProtocol.Interfaces.Servers;
    using FlowProtocol.Interfaces.Workers;
    using Ninject;

    public class IoC
    {
        private static readonly IKernel Kernel = new StandardKernel();

        public static void RegisterAll()
        {
            Kernel.Bind<IFlowProtocolRequestParser>()
                .To<RequestParser>();

            Kernel.Bind<IServer>()
                .To<FlowUdpServer>();

            Kernel.Bind<IFlowServerWorker>()
                .To<UdpServerWorker>()
                .WithConstructorArgument(
                    "parser",
                    Kernel.Get<IFlowProtocolRequestParser>());
        }

        public static T Resolve<T>()
        {
            return Kernel.Get<T>();
        }
    }
}