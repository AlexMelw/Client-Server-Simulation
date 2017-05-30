namespace FlowProtocol.Implementation.Workers.Servers
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using Interfaces.Request;
    using Interfaces.Workers;
    using Ninject;
    using ProtocolHelpers;
    using Request;

    public class UdpServerWorker : IFlowServerWorker
    {
        private readonly IFlowProtocolRequestProcessor _requestProcessor;

        private IPEndPoint _remoteClientEndPoint;
        private UdpClient _server;

        public static UdpServerWorker Instance => new UdpServerWorker(new RequestProcessor(new RequestParser()));

        #region CONSTRUCTORS

        private UdpServerWorker(IFlowProtocolRequestProcessor requestProcessor)
        {
            _requestProcessor = requestProcessor;
        }

        #endregion

        public void ExecuteRequest(string requestString)
        {
            new Thread(() =>
            {
                Console.Out.WriteLine($" [UDP]  SERVER WORKER IS TALKING TO {_remoteClientEndPoint}");

                string result = _requestProcessor.ProcessRequest(requestString);

                byte[] buffer = result.ToFlowProtocolAsciiEncodedBytesArray();

                _server.Send(buffer, buffer.Length, _remoteClientEndPoint);

                Console.Out.WriteLine($" [UDP]  SERVER WORKER for {_remoteClientEndPoint} finished job");
            }).Start();
        }

        public UdpServerWorker Init(IPEndPoint remoteClientEndPoint, UdpClient udpServer)
        {
            _remoteClientEndPoint = remoteClientEndPoint;
            _server = udpServer;
            return this;
        }
    }
}