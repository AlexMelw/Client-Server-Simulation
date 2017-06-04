namespace Infrastructure
{
    using FlowProtocol.Implementation.Response;
    using FlowProtocol.Implementation.Workers.Clients;
    using FlowProtocol.Interfaces.Response;
    using FlowProtocol.Interfaces.Workers;
    using FlowProtocol.Interfaces.Workers.Clients;
    using Ninject;

    public class IoC
    {
        private static readonly IKernel Kernel = new StandardKernel();

        public static void RegisterAll()
        {
            Kernel.Bind<IFlowProtocolResponseParser>()
                .To<ResponseParser>();

            Kernel.Bind<IFlowUdpClientWorker>()
                .To<UdpClientWorker>();

            Kernel.Bind<IFlowTcpClientWorker>()
                .To<TcpClientWorker>();

            //Kernel.Bind<UdpClientWorker>()
            //    .To<UdpClientWorker>()
            //    .WithConstructorArgument(
            //        "parser",
            //        Resolve<IFlowProtocolResponseParser>());
        }

        public static T Resolve<T>() => Kernel.Get<T>();
    }
}