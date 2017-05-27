namespace FlowProtocol.Implementation.Workers.Servers
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using EasySharp.NHelpers;
    using Interfaces;
    using Interfaces.Request;
    using Ninject;
    using Request;
    using TranslatorReference;
    using static Interfaces.CommonConventions.Conventions;

    public class UdpServerWorker : IFlowServerWorker
    {
        private const string AppId = "6CE9C85A41571C050C379F60DA173D286384E0F2";
        private readonly IFlowProtocolRequestParser _parser;
        private IPEndPoint _remoteClientEndPoint;
        private UdpClient _udpServer;
        public static UdpServerWorker Instance => new UdpServerWorker(new RequestParser());

        #region CONSTRUCTORS

        [Inject]
        public UdpServerWorker(IFlowProtocolRequestParser parser)
        {
            _parser = parser;
        }

        #endregion

        public void ProcessRequest(string request)
        {
            new Thread(() =>
            {
                Console.Out.WriteLine($"[ UDP ] SERVER WORKER IS TALKING TO {_remoteClientEndPoint}");

                string result = ExecuteRequest(request);

                byte[] bufferByteArray = $"OK 200 [ {result} ]".ToAsciiEncodedByteArray();
                _udpServer.Send(bufferByteArray, bufferByteArray.Length, _remoteClientEndPoint);

                Console.Out.WriteLine($"[ UDP ] SERVER WORKER for {_remoteClientEndPoint} finished job");
            }).Start();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        private string ExecuteRequest(string request)
        {
            ConcurrentDictionary<string, string> requestMembers = _parser.ParseRequest(request);

            if (requestMembers == null)
                return BadRequest;

            if (requestMembers.TryGetValue(Cmd, out string cmd))
            {
                if (cmd == Commands.Translate)
                {
                    requestMembers.TryGetValue(SourceText, out string sourceText);
                    requestMembers.TryGetValue(SourceLang, out string sourceLang);
                    requestMembers.TryGetValue(TargetLang, out string targetLang);

                    string translatedText = Translate(
                        sourceText: sourceText,
                        sourceLang: sourceLang,
                        targetLang: targetLang
                    );
                    return
                        $@"200 OK TRANSLATE --TEXT='{translatedText}'";
                }
                if (cmd == Commands.Auth) { }
                if (cmd == Commands.Register) { }
            }
            return BadRequest;
        }

        public string Translate(string sourceText, string sourceLang, string targetLang)
        {
            try
            {
                var translatorClient = new LanguageServiceClient();

                string from = (targetLang == Lang.Unknown ? "" : targetLang);
                string to = (targetLang == Lang.Unknown ? Lang.English : targetLang);

                string translatedText = translatorClient.Translate(
                    "6CE9C85A41571C050C379F60DA173D286384E0F2",
                    sourceText,
                    $"{from}",
                    $"{to}");


                return translatedText;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        ~UdpServerWorker()
        {
            ReleaseUnmanagedResources();
        }

        public UdpServerWorker Init(IPEndPoint remoteClientEndPoint, UdpClient udpServer)
        {
            _remoteClientEndPoint = remoteClientEndPoint;
            _udpServer = udpServer;
            return this;
        }
    }
}