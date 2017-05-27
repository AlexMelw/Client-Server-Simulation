namespace FlowProtocol.Implementation.Servers
{
    using System.Threading;

    public class FlowTcpServer
    {
        public void StartListeningToPort(int port)
        {
            new Thread(() =>
            {
                //IPAddress ipAddress = IPAddress.Parse(Localhost);
                //TcpListener tcpListener = new TcpListener(ipAddress, tcpServerListeningPort);

                //try
                //{
                //    tcpListener.Start();

                //    Console.WriteLine($" The server is running at port {tcpServerListeningPort}...");
                //    Console.WriteLine(" The local End point is  :" + tcpListener.LocalEndpoint);
                //    Console.WriteLine(" Waiting for a connection.....");
                //    Console.Out.WriteLine();

                //    int count = 0;
                //    while (true)
                //    {
                //        Socket workerTcpSocket = tcpListener.AcceptSocket();

                //        Console.WriteLine(" Connection accepted from " + workerTcpSocket.RemoteEndPoint);

                //        // The IP header and the TCP header take up 20 bytes each at least
                //        // (unless optional header fields are used) and thus the max for
                //        // (non-Jumbo frame) Ethernet is 1500 - 20 -20 = 1460
                //        // +12 UDP Overhead (if I want to do a TCP/UDP universal buffer)
                //        byte[] buffer = new byte[1472];

                //        while (true)
                //        {
                //            count++;
                //            int receivedBytes = workerTcpSocket.Receive(buffer);

                //            if (receivedBytes == 0)
                //            {
                //                workerTcpSocket.Close();
                //                Console.Out.WriteLine(" No bytes received. Connection closed.");
                //                break;
                //            }

                //            Console.WriteLine(" Recieved some bytes...");

                //            //for (int i = 0; i < receivedBytes; i++)
                //            //    Console.Write(Convert.ToChar(buffer[i]));

                //            string requestString = buffer.Take(receivedBytes).ToArray().ToFlowProtocolAsciiDecodedString();

                //            if (requestString == "close")
                //            {
                //                workerTcpSocket.Send(CloseConnection.ToFlowProtocolAsciiEncodedBytesArray());
                //                workerTcpSocket.Close();
                //                Console.Out.WriteLine(" Connection closed by client's intent.");
                //                //Console.ReadLine();
                //                break;
                //            }

                //            Console.Out.WriteLine($" REMOTE MESSAGE: {requestString}");

                //            //ASCIIEncoding asen = new ASCIIEncoding();

                //            workerTcpSocket.Send(" 200 OK [ Message Received ]".ToFlowProtocolAsciiEncodedBytesArray());

                //            if ( /*stop server command received*/ count == 5)
                //            {
                //                Console.Out.WriteLine("Server has ended serving requests for given client.");
                //                workerTcpSocket.Close();
                //                //Console.ReadLine();
                //                break;
                //            }
                //        }
                //        count = 0;
                //        Console.Out.WriteLine("SERVER HALTED");
                //        //Console.ReadLine();
                //    }
                //}
                //catch (Exception e)
                //{
                //    Console.Out.WriteLine("Grave error occured. Searver is dead.");
                //    Console.Out.WriteLine($"e = {e}");
                //    Debug.WriteLine("Grave error occured. Searver is dead.");
                //    Debug.WriteLine($"e = {e}");
                //    Console.ReadLine();
                //    throw;
                //}
                //finally
                //{
                //    tcpListener.Stop();
                //}
            }).Start();
        }
    }
}