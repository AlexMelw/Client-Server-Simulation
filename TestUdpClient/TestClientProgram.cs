using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUdpClient
{
    using System.Net;
    using System.Net.Sockets;
    using EasySharp.NHelpers;
    using static FlowProtocol.Interfaces.CommonConventions.Conventions;

    class TestClientProgram
    {
        static void Main(string[] args)
        {
            Console.Out.WriteLine("Client...");

            var remoteIpAddress = IPAddress.Parse(Localhost);

            var serverEndPoint = new IPEndPoint(remoteIpAddress, UdpServerListeningPort);

            var udpClient = new UdpClient();

            udpClient.Connect(serverEndPoint);

            for (
                string cmdLine = Console.ReadLine();
                cmdLine != Quit;
                cmdLine = Console.ReadLine())
            {
                byte[] bufferByteArray = cmdLine.ToAsciiEncodedByteArray();
                udpClient.Send(bufferByteArray, bufferByteArray.Length);

                bufferByteArray = udpClient.Receive(ref serverEndPoint);
                Console.Out.WriteLine($"{bufferByteArray.ToAsciiString()}");
            }
        }
    }
}