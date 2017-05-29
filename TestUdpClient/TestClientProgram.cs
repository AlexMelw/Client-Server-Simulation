namespace TestUdpClient
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using FlowProtocol.Implementation.ProtocolHelpers;
    using static FlowProtocol.Interfaces.CommonConventions.Conventions;

    public class TestClientProgram
    {
        internal static void Main(string[] args)
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
                byte[] bufferByteArray = cmdLine.ToFlowProtocolAsciiEncodedBytesArray();
                udpClient.Send(bufferByteArray, bufferByteArray.Length);

                bufferByteArray = udpClient.Receive(ref serverEndPoint);
                Console.Out.WriteLine($"{bufferByteArray.ToFlowProtocolAsciiDecodedString()}");
            }
        }
    }
}