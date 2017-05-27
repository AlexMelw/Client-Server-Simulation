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
    using MSTranslatorService;
    using Ninject;
    using Request;
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

                byte[] bufferByteArray = result.ToAsciiEncodedByteArray();
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
                        sourceText,
                        sourceLang,
                        targetLang
                    );
                    return $@"200 OK TRANSLATE --TEXT='{translatedText}'";
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
                var translatorClient = new SoapService();

                string from = sourceLang == Lang.Unknown ? "" : sourceLang;
                string to = targetLang == Lang.Unknown ? Lang.English : targetLang;

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

namespace FlowProtocol.Implementation.GoGoService
{
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading.Tasks;

    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [ServiceContract(Namespace = "http://api.microsofttranslator.com/v1/soap.svc",
        ConfigurationName = "GoGoService.LanguageService")]
    public interface LanguageService
    {
        [OperationContract(Action = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/GetLanguages",
            ReplyAction = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/GetLanguagesRespon" +
                          "se")]
        string[] GetLanguages(string appId);

        [OperationContract(Action = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/GetLanguages",
            ReplyAction = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/GetLanguagesRespon" +
                          "se")]
        Task<string[]> GetLanguagesAsync(string appId);

        [OperationContract(Action = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/GetLanguageNames",
            ReplyAction = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/GetLanguageNamesRe" +
                          "sponse")]
        string[] GetLanguageNames(string appId, string locale);

        [OperationContract(Action = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/GetLanguageNames",
            ReplyAction = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/GetLanguageNamesRe" +
                          "sponse")]
        Task<string[]> GetLanguageNamesAsync(string appId, string locale);

        [OperationContract(Action = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/Detect",
            ReplyAction = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/DetectResponse")]
        string Detect(string appId, string text);

        [OperationContract(Action = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/Detect",
            ReplyAction = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/DetectResponse")]
        Task<string> DetectAsync(string appId, string text);

        [OperationContract(Action = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/Translate",
            ReplyAction = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/TranslateResponse")]
        string Translate(string appId, string text, string from, string to);

        [OperationContract(Action = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/Translate",
            ReplyAction = "http://api.microsofttranslator.com/v1/soap.svc/LanguageService/TranslateResponse")]
        Task<string> TranslateAsync(string appId, string text, string from, string to);
    }

    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public interface LanguageServiceChannel : LanguageService, IClientChannel { }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public class LanguageServiceClient : ClientBase<LanguageService>, LanguageService
    {
        #region CONSTRUCTORS

        public LanguageServiceClient() { }

        public LanguageServiceClient(string endpointConfigurationName) :
            base(endpointConfigurationName) { }

        public LanguageServiceClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress) { }

        public LanguageServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress) { }

        public LanguageServiceClient(Binding binding, EndpointAddress remoteAddress) :
            base(binding, remoteAddress) { }

        #endregion

        public string[] GetLanguages(string appId)
        {
            return Channel.GetLanguages(appId);
        }

        public Task<string[]> GetLanguagesAsync(string appId)
        {
            return Channel.GetLanguagesAsync(appId);
        }

        public string[] GetLanguageNames(string appId, string locale)
        {
            return Channel.GetLanguageNames(appId, locale);
        }

        public Task<string[]> GetLanguageNamesAsync(string appId, string locale)
        {
            return Channel.GetLanguageNamesAsync(appId, locale);
        }

        public string Detect(string appId, string text)
        {
            return Channel.Detect(appId, text);
        }

        public Task<string> DetectAsync(string appId, string text)
        {
            return Channel.DetectAsync(appId, text);
        }

        public string Translate(string appId, string text, string from, string to)
        {
            return Channel.Translate(appId, text, from, to);
        }

        public Task<string> TranslateAsync(string appId, string text, string from, string to)
        {
            return Channel.TranslateAsync(appId, text, from, to);
        }
    }
}