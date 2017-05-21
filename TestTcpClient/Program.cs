using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTcpClient
{
    using System.Net.Sockets;
    using EasySharp.NHelpers;

    class Program
    {
        static void Main(string[] args)
        {
            TcpClient tcpClient = new TcpClient();
            Console.WriteLine("Connecting to server");

            tcpClient.Connect("127.0.0.1", 5150);
            Console.WriteLine("Connected");
            Console.WriteLine("enter The string to be  transmitted");

            string textToTransmit = string.Empty;

            while (textToTransmit != "quit")
            {
                textToTransmit = Console.ReadLine();

                NetworkStream tcpStream = tcpClient.GetStream();
                byte[] bytesArray = textToTransmit.ToAsciiEncodedByteArray();

                Console.WriteLine("Transmitting.....");

                tcpStream.Write(bytesArray, 0, bytesArray.Length);

                byte[] bufferArray = new byte[1472];

                int bytesRead = tcpStream.Read(bufferArray, 0, 1472);

                for (int i = 0; i < bytesRead; i++)
                {
                    Console.Write(Convert.ToChar(bufferArray[i]));
                }
            }
            tcpClient.Close();
        }
    }
}