namespace Protocol.Implementation
{
    using System.Collections.Concurrent;
    using System.Text.RegularExpressions;
    using Interfaces;

    public class ResponseParser : ICommunicationProtocolResponseProcessor
    {
        private const string StatusCode = "StatusCode";
        private const string ObjectType = "objectType";
        private const string ObjectValue = "objectValue";
        private const string StatusDescription = "StatusDescription";

        private readonly string _pattern = @"(THIS IS A PATTERN)";

        public string ProcessResponseGetImageSrc(string response)
        {
            var responseComponents = ParseResponse(response);

            /* PROCESS RESPONSE */

            // Maybe deserialize JSON/XML and get image src.

            return "IMAGE SOURCE (SRC='http://example.com/image.png')";
        }
        //private ConcurrentDictionary<string, string> responseComponents;

        private ConcurrentDictionary<string, string> ParseResponse(string response)
        {
            Regex parser = new Regex(_pattern);
            Match match = parser.Match(response);

            ConcurrentDictionary<string, string> responseComponents = new ConcurrentDictionary<string, string>();
            if (match.Success)
            {
                responseComponents.TryAdd(StatusCode, match.Groups[StatusCode].Value);
                responseComponents.TryAdd(StatusDescription, match.Groups[StatusDescription].Value);
                responseComponents.TryAdd(ObjectType, match.Groups[ObjectType].Value);
                responseComponents.TryAdd(ObjectValue, match.Groups[ObjectValue].Value);

                return responseComponents;
            }
            return null;
        }
    }
}