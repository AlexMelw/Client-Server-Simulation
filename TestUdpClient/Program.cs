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

    class Program
    {
        private const string IpAddress = "127.0.0.1";
        private const int ServerUdpListeningPort = 6401;

        static void Main(string[] args)
        {
            // Client ---------------------------
            Console.Out.WriteLine("Client...");
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(IpAddress), ServerUdpListeningPort);
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(serverEndPoint);

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
    }
}