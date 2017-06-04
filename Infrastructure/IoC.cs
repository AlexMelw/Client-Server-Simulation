namespace Infrastructure
{
    using FlowProtocol.Implementation.Response;
    using FlowProtocol.Implementation.Workers.Clients;
    using FlowProtocol.Interfaces.Response;
    using Ninject;

    public class IoC
    {
        private static readonly IKernel Kernel = new StandardKernel();

        public static void RegisterAll()
        {
            Kernel.Bind<IFlowProtocolResponseParser>()
                .To<ResponseParser>();

            Kernel.Bind<UdpClientWorker>()
                .To<UdpClientWorker>();

            //Kernel.Bind<UdpClientWorker>()
            //    .To<UdpClientWorker>()
            //    .WithConstructorArgument(
            //        "parser",
            //        Resolve<IFlowProtocolResponseParser>());
        }

        public static T Resolve<T>() => Kernel.Get<T>();
    }
}