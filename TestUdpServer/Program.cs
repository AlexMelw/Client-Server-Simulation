﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUdpServer
{
    using System.Net;
    using System.Net.Sockets;
    using EasySharp.NHelpers;

    class Program
    {
        private const int ServerUdpListeningPort = 6401;

        static void Main(string[] args)
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
                Console.Out.WriteLine($"Remote Message: {cmdLine}");

                bufferBytesArray = "OK 200 [ Message Received ]".ToAsciiEncodedByteArray();
                udpServer.Send(bufferBytesArray, bufferBytesArray.Length, serverEndPoint);
            }
        }
    }
}
