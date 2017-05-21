namespace Protocol.Implementation.Request
{
    using System;
    using System.Diagnostics;
    using Interfaces;
    using Interfaces.Request;
    using TranslatorService;

    public class RequestProcessor : ICommunicationProtocolRequestProcessor
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
                    appId: "6CE9C85A41571C050C379F60DA173D286384E0F2",
                    text: sourceText,
                    @from: "",
                    to: "en");

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
    }
}