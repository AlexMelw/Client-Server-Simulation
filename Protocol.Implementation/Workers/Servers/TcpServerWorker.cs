namespace FlowProtocol.Implementation.Workers.Servers
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using Interfaces;
    using TranslatorService;

    public class TcpServerWorker : IFlowServerWorker
    {
        public void ProcessRequest(string response)
        {
            throw new NotImplementedException();
        }

        public string Translate(string sourceText)
        {
            try
            {
                var translatorClient = new LanguageServiceClient();

                translatorClient.Translate(
                    "6CE9C85A41571C050C379F60DA173D286384E0F2",
                    sourceText,
                    "",
                    "en");


                //todo
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        public void Send(string message)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init(IPAddress ipAddress, int port)
        {
            throw new NotImplementedException();
        }

        public void StartServing()
        {
            throw new NotImplementedException();
        }
    }
}