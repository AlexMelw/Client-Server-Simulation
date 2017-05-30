namespace FlowProtocol.Implementation.Workers.Servers
{
    using System;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading;
    using EasySharp.NHelpers.Wrappers;
    using Interfaces.Request;
    using Interfaces.Workers;
    using ProtocolHelpers;
    using Request;
    using static Interfaces.CommonConventions.Conventions;

    public class TcpServerWorker : IFlowServerWorker
    {
        private readonly IFlowProtocolRequestProcessor _requestProcessor;
        private TcpListenerEx _server;

        private Socket _workerSocket;

        public static TcpServerWorker Instance => new TcpServerWorker(new RequestProcessor(new RequestParser()));

        #region CONSTRUCTORS

        private TcpServerWorker(IFlowProtocolRequestProcessor requestProcessor)
        {
            _requestProcessor = requestProcessor;
        }

        #endregion

        public void StartWorking()
        {
            new Thread(() =>
            {
                Console.Out.WriteLine($"[ TCP ] SERVER WORKER IS TALKING TO {_workerSocket.RemoteEndPoint}");

                // The IP header and the TCP header take up 20 bytes each at least
                // (unless optional header fields are used) and thus the max for
                // (non-Jumbo frame) Ethernet is 1500 - 20 -20 = 1460
                // +12 UDP Overhead (if I want to do a TCP/UDP universal buffer)

                bool serverMustStopServingRequests = false;
                bool connectionAlive = true;

                while (connectionAlive)
                {
                    if (_server.Inactive)
                    {
                        connectionAlive = false;
                        continue;
                    }

                    byte[] buffer = new byte[EthernetTcpUdpPacketSize];
                    int receivedBytes = _workerSocket.Receive(buffer);

                    if (receivedBytes == 0)
                    {
                        _workerSocket.Close();
                        connectionAlive = false;

                        Console.Out.WriteLine(
                            $@"[ TCP ] SERVER WORKER BOUND TO {
                                    _workerSocket.RemoteEndPoint
                                } says: ""No bytes received. Connection closed.""");

                        continue;
                    }

                    //Console.WriteLine(" Recieved some bytes...");

                    //for (int i = 0; i < receivedBytes; i++)
                    //    Console.Write(Convert.ToChar(buffer[i]));

                    string requestString = buffer
                        .Take(receivedBytes)
                        .ToArray()
                        .ToFlowProtocolAsciiDecodedString();

                    if (requestString == CloseConnection)
                    {
                        connectionAlive = false;
                        Console.Out.WriteLine($"Client [ {_workerSocket.RemoteEndPoint} ] closed connection");
                        continue;
                    }

                    if (requestString == QuitServerCmd)
                    {
                        connectionAlive = false;
                        serverMustStopServingRequests = true;

                        Console.Out.WriteLine($"Client [ {_workerSocket.RemoteEndPoint} ] closed connection");
                        Console.Out.WriteLine("Connection closed by client's intent.");
                        continue;
                    }

                    Console.Out.WriteLine($"[TCP] Remote Message: {requestString}");

                    string result = _requestProcessor.ProcessRequest(requestString);
                    buffer = result.ToFlowProtocolAsciiEncodedBytesArray();

                    // SEND BACK A RESPONSE
                    _workerSocket.Send(buffer);
                }

                _workerSocket.Close();

                if (serverMustStopServingRequests && _server.Active)
                {
                    _server.Stop();
                }

                Console.Out.WriteLine($"[ TCP ] SERVER WORKER for {_workerSocket.RemoteEndPoint} finished job");
            }).Start();
        }

        public TcpServerWorker Init(Socket workerTcpSocket, TcpListenerEx tcpListener)
        {
            _workerSocket = workerTcpSocket;
            _server = tcpListener;
            return this;
        }
    }
}