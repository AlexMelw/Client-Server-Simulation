namespace FlowProtocol.Implementation.Workers.Servers
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using EasySharp.NHelpers;
    using Interfaces;
    using Ninject;
    using Request;

    public class UdpServerWorker : IFlowServerWorker
    {
        private readonly IFlowProtocolRequestProcessor _requestProcessor;

        private IPEndPoint _remoteClientEndPoint;
        private UdpClient _udpServer;

        public static UdpServerWorker Instance => new UdpServerWorker(new RequestProcessor(new RequestParser()));

        #region CONSTRUCTORS

        [Inject]
        public UdpServerWorker(IFlowProtocolRequestProcessor requestProcessor)
        {
            _requestProcessor = requestProcessor;
        }

        #endregion

        public void ExecuteRequest(string request)
        {
            new Thread(() =>
            {
                Console.Out.WriteLine($"[ UDP ] SERVER WORKER IS TALKING TO {_remoteClientEndPoint}");

                string result = _requestProcessor.ProcessRequest(request);

                byte[] bufferByteArray = result.ToAsciiEncodedByteArray();

                _udpServer.Send(bufferByteArray, bufferByteArray.Length, _remoteClientEndPoint);

                Console.Out.WriteLine($"[ UDP ] SERVER WORKER for {_remoteClientEndPoint} finished job");
            }).Start();
        }

        public UdpServerWorker Init(IPEndPoint remoteClientEndPoint, UdpClient udpServer)
        {
            _remoteClientEndPoint = remoteClientEndPoint;
            _udpServer = udpServer;
            return this;
        }
    }
}