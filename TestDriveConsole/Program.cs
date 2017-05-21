using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestDriveConsole
{
    using System.Net;
    using System.Net.Sockets;
    using System.Text.RegularExpressions;
    using EasySharp.NHelpers;

    class Program
    {
        private const int ServerUdpListeningPort = 6401;

        static void Main(string[] args)
        {

        }

        static void ClientLife()
        {
            // Client ---------------------------
            Console.Out.WriteLine("Client...");
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), ServerUdpListeningPort);
            UdpClient udpClient = new UdpClient();

            for (
                string cmdLine = Console.ReadLine();
                cmdLine != "quit";
                cmdLine = Console.ReadLine())
            {
                byte[] bufferByteArray = cmdLine.ToAsciiEncodedByteArray();
                udpClient.Send(bufferByteArray, bufferByteArray.Length);

                bufferByteArray = udpClient.Receive(ref serverEndPoint);
                Console.Out.WriteLine($"{bufferByteArray.ToAsciiString()}");
            }
        }

        static void ServerLife()
        {
            // Server --------------------------
            Console.Out.WriteLine("Server...");
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, ServerUdpListeningPort);
            UdpClient udpServer = new UdpClient(ServerUdpListeningPort);

            for (
                string cmdLine = string.Empty;
                cmdLine != "quit server";)
            {
                byte[] bufferBytesArray = udpServer.Receive(ref serverEndPoint);
                cmdLine = bufferBytesArray.ToAsciiString();
                Console.Out.WriteLine("bufferBytesArray = {0}", cmdLine);

                bufferBytesArray = "OK 200".ToAsciiEncodedByteArray();
                udpServer.Send(bufferBytesArray, bufferBytesArray.Length, serverEndPoint);
            }
        }
    }
}