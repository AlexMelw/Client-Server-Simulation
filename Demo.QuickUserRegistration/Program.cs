using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.QuickUserRegistration
{
    using System.Diagnostics;
    using System.Net;
    using FlowProtocol.Implementation.Response;
    using FlowProtocol.Implementation.Workers.Clients;
    using FlowProtocol.Interfaces.CommonConventions;
    using FlowProtocol.Interfaces.Workers;

    class Program
    {
        static void Main(string[] args)
        {
            IFlowClientWorker clientWorker = new TcpClientWorker(new ResponseParser());

            bool registered = false;

            try
            {
                clientWorker.Connect(
                    ipAddress: IPAddress.Parse(Conventions.Localhost),
                    port: Conventions.TcpServerListeningPort
                );

                registered = clientWorker.Register(
                    login: "Admin",
                    password: "qwerty123",
                    name: "Veaceslav BARBARII"
                );

                registered = clientWorker.Register(
                    login: "Demo1",
                    password: "qwerty",
                    name: "Guba Dumitru"
                );

                registered = clientWorker.Register(
                    login: "Demo2",
                    password: "qwerty",
                    name: "Bujac Petru"
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

            Console.ReadLine();
        }
    }
}