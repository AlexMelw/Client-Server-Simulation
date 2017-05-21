using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTcpServer
{
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using EasySharp.NHelpers;

    class Program
    {
        private const string CloseConnection = "close connection";

        static void Main(string[] args)
        {
            try
            {
                const int port = 5150;
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                TcpListener tcpListener = new TcpListener(ipAddress, port);
                tcpListener.Start();

                Console.WriteLine($" The server is running at port {port}...");
                Console.WriteLine(" The local End point is  :" + tcpListener.LocalEndpoint);
                Console.WriteLine(" Waiting for a connection.....");
                Console.Out.WriteLine();

                int count = 0;
                while (true)
                {
                    Socket workerTcpSocket = tcpListener.AcceptSocket();
                    Console.WriteLine(" Connection accepted from " + workerTcpSocket.RemoteEndPoint);

                    // The IP header and the TCP header take up 20 bytes each at least
                    // (unless optional header fields are used) and thus the max for
                    // (non-Jumbo frame) Ethernet is 1500 - 20 -20 = 1460
                    // +12 UDP Overhead (if I want to do a TCP/UDP universal buffer)
                    byte[] buffer = new byte[1472];

                    while (true)
                    {
                        count++;
                        int receivedBytes = workerTcpSocket.Receive(buffer);

                        if (receivedBytes == 0)
                        {
                            workerTcpSocket.Close();
                            Console.Out.WriteLine(" No bytes received. Connection closed.");
                            break;
                        }

                        Console.WriteLine(" Recieved some bytes...");

                        //for (int i = 0; i < receivedBytes; i++)
                        //    Console.Write(Convert.ToChar(buffer[i]));

                        string responseString = buffer.Take(receivedBytes).ToArray().ToAsciiString();

                        if (responseString == "close")
                        {
                            workerTcpSocket.Send(CloseConnection.ToAsciiEncodedByteArray());
                            workerTcpSocket.Close();
                            Console.Out.WriteLine(" Connection closed by client's intent.");
                            Console.ReadLine();
                        }

                        Console.Out.WriteLine($" REMOTE MESSAGE: {responseString}");

                        //ASCIIEncoding asen = new ASCIIEncoding();

                        workerTcpSocket.Send(" 200 OK [ Message Received ]".ToAsciiEncodedByteArray());

                        if ( /*stop server command received*/ count == 5)
                        {
                            Console.Out.WriteLine("Server has ended serving requests for given client.");
                            workerTcpSocket.Close();
                            tcpListener.Stop();
                            Console.ReadLine();
                            break;
                        }
                    }
                    Console.Out.WriteLine("SERVER HALTED");
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Grave error occured");
                Console.Out.WriteLine("e = {0}", e);
                Debug.WriteLine("Grave error occured");
                throw;
                Console.ReadLine();
            }
        }
    }
}
