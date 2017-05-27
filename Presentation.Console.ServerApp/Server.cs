namespace Presentation.Console.ServerApp
{
    using System;
    using FlowProtocol.Implementation.Servers;
    using Infrastructure;
    using static FlowProtocol.Interfaces.CommonConventions.Conventions;

    public class Server
    {
        //#region CONSTRUCTORS

        //static Server()
        //{
        //    IoC.RegisterAll();
        //}

        //#endregion

        private static void Main(string[] args)
        {
            //var flowServer = IoC.Resolve<IServer>();

            var flowUdpServer = new FlowUdpServer();
            var flowTcpServer = new FlowTcpServer();

            flowUdpServer.StartListeningToPort(UdpServerListeningPort);
            flowTcpServer.StartListeningToPort(TcpServerListeningPort);


            //var translatorClient = new LanguageServiceClient();

            //string translated = translatorClient.Translate(
            //    appId: "6CE9C85A41571C050C379F60DA173D286384E0F2",
            //    text: "Ce mai faci?",
            //    @from: "",
            //    to: "en"); 


            //Console.Out.WriteLine("translated = {0}", translated);
            //Console.ReadLine();
        }
    }
}