namespace Protocol.Implementation.Request
{
    using System;
    using System.Diagnostics;
    using Interfaces.Request;
    using TranslatorService;

    public class RequestProcessor : ICommunicationProtocolRequestProcessor
    {
        private readonly RequestParser _requestParser;

        #region CONSTRUCTORS

        public RequestProcessor(RequestParser requestParser)
        {
            _requestParser = requestParser;
        }

        #endregion

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
    }
}