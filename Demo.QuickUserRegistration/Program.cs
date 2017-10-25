namespace Demo.QuickUserRegistration
{
    using System;
    using System.Net;
    using System.Threading;
    using FlowProtocol.Implementation.Response;
    using FlowProtocol.Implementation.Workers.Clients;
    using FlowProtocol.Interfaces.CommonConventions;
    using FlowProtocol.Interfaces.Workers.Clients;

    internal class Program
    {
        private static void Main(string[] args)
        {
            IFlowClientWorker clientWorker = new TcpClientWorker(new ResponseParser());

            bool registered = false;

            try
            {
                clientWorker.Connect(
                    IPAddress.Parse(Conventions.Localhost),
                    Conventions.TcpServerListeningPort
                );

                registered = clientWorker.Register(
                    "Admin",
                    "qwerty",
                    "Veaceslav BARBARII"
                );

                registered = clientWorker.Register(
                    "Demo1",
                    "q",
                    "Guba Dumitru"
                );

                registered = clientWorker.Register(
                    "Demo2",
                    "q",
                    "Bujac Petru"
                );
            }
            catch (Exception)
            {
                Console.Out.WriteLine("Haven't succeded to register users...");
            }
            finally
            {
                clientWorker.Dispose();
            }

            Console.Out.WriteLine(registered ? "Users are registered..." : "Users are not registered...");

            Thread.Sleep(5000);
            Environment.Exit(0);
        }
    }
}