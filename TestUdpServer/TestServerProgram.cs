namespace TestUdpServer
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using FlowProtocol.Implementation.ProtocolHelpers;
    using static FlowProtocol.Interfaces.CommonConventions.Conventions;

    class TestServerProgram
    {
        static void Main(string[] args)
        {
            //SINGLE THREAD
            // Server --------------------------
            Console.Out.WriteLine("Server...");
            IPEndPoint remoteClientEndPoint = new IPEndPoint(IPAddress.None, 0);
            UdpClient udpServer = new UdpClient(
                new IPEndPoint(IPAddress.Any, UdpServerListeningPort));

            for (
                string cmdLine = string.Empty;
                cmdLine != "quit server";)
            {
                byte[] bufferBytesArray = udpServer.Receive(ref remoteClientEndPoint);
                cmdLine = bufferBytesArray.ToFlowProtocolAsciiDecodedString();
                Console.Out.WriteLine($"Remote Message: {cmdLine}");

                bufferBytesArray = "OK 200 [ Message Received ]".ToFlowProtocolAsciiEncodedBytesArray();
                udpServer.Send(bufferBytesArray, bufferBytesArray.Length, remoteClientEndPoint);
            }
        }
    }
}