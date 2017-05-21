using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTcpClient
{
    using System.Net;
    using System.Net.Sockets;
    using EasySharp.NHelpers;
    using Protocol.Interfaces.ProtocolHelpers;

    class Program
    {
        private const string Localhost = "127.0.0.1";
        private const string CloseConnection = "close connection";

        static void Main(string[] args)
        {
            TcpClient tcpClient = new TcpClient();
            Console.WriteLine(" Connecting to server");

            tcpClient.Connect(IPAddress.Parse(Localhost), 5150);
            Console.WriteLine(" Connected");

            string textToTransmit = string.Empty;

            while (textToTransmit != "quit")
            {
                Console.WriteLine(" Enter The string to be transmitted: ");
                textToTransmit = Console.ReadLine();

                if (!tcpClient.Connected)
                {
                    Console.Out.WriteLine(" Client is not connected to the server!");
                    Console.ReadLine();
                    tcpClient.Close();
                }

                NetworkStream tcpStream = tcpClient.GetStream();


                byte[] bytesArray = textToTransmit.ToFlowProtocolAsciiEncodedBytesArray();

                Console.WriteLine(" Transmitting.....");

                tcpStream.Write(bytesArray, 0, bytesArray.Length);

                byte[] bufferArray = new byte[1472];

                int bytesRead = tcpStream.Read(bufferArray, 0, 1472);

                string serverResponse = bufferArray.Take(bytesRead).ToArray().ToFlowProtocolAsciiDecodedString();

                if (serverResponse == CloseConnection)
                {
                    Console.Out.WriteLine(" Server is no more serving requests");
                    Console.ReadLine();
                    break;
                }
            }
            tcpClient.Close();
        }
    }
}