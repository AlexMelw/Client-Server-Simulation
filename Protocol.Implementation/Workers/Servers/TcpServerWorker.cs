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
                Console.Out.WriteLine($" [TCP] SERVER WORKER IS TALKING TO {_workerSocket.RemoteEndPoint}");

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
                            $@" [TCP] SERVER WORKER says: ""No bytes received. Connection closed.""");

                        continue;
                    }

                    string requestString = buffer
                        .Take(receivedBytes)
                        .ToArray()
                        .ToFlowProtocolAsciiDecodedString();

                    Console.Out.WriteLine($" [TCP] Remote Message: {requestString}");

                    if (requestString == CloseConnection)
                    {
                        connectionAlive = false;
                        Console.Out.WriteLine($" Client closed connection");
                        continue;
                    }

                    if (requestString == QuitServerCmd)
                    {
                        connectionAlive = false;
                        serverMustStopServingRequests = true;

                        _workerSocket.Send("200 OK SHUTDOWN --res='TCP Server Halted'"
                            .ToFlowProtocolAsciiEncodedBytesArray());

                        Console.Out.WriteLine($" Client closed connection");
                        Console.Out.WriteLine(" Client turned off [ TCP ] server.");
                        continue;
                    }

                    // OTHERWISE PROCESS REQUEST
                    string result = _requestProcessor.ProcessRequest(requestString);
                    buffer = result.ToFlowProtocolAsciiEncodedBytesArray();

                    // SEND BACK A RESPONSE
                    _workerSocket.Send(buffer);
                }

                _workerSocket.Close();

                if (serverMustStopServingRequests && _server.Active)
                {
                    _server.Stop();
                    Console.Out.WriteLine(" [TCP] SERVER HALTED");
                }

                Console.Out.WriteLine($" [TCP] SERVER WORKER finished job");
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