namespace FlowProtocol.Implementation.Workers.Servers
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using Interfaces;
    using Interfaces.Workers;

    public class TcpServerWorker : IFlowServerWorker
    {
        public string Translate(string sourceText)
        {
            //try
            //{
            //    var translatorClient = new LanguageServiceClient();

            //    translatorClient.Translate(
            //        "6CE9C85A41571C050C379F60DA173D286384E0F2",
            //        sourceText,
            //        "",
            //        "en");


            //    //todo
            //    return null;
            //}
            //catch (Exception e)
            //{
            //    Debug.WriteLine(e);
            //    throw;
            //}
            return null;
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        public void ExecuteRequest(string request)
        {
            throw new NotImplementedException();
        }

        ~TcpServerWorker() {
            ReleaseUnmanagedResources();
        }
    }
}