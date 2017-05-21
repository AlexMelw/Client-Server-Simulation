namespace Protocol.Implementation.Response
{
    using System.Collections.Concurrent;
    using System.Text.RegularExpressions;
    using Interfaces.Response;

    public class ResponseParser : ICommunicationProtocolResponseParser
    {
        private const long Size10MB = 10 * 1024 * 1024L;
        private const string StatusCode = "StatusCode";
        private const string ObjectType = "objectType";
        private const string ObjectValue = "objectValue";
        private const string StatusDescription = "StatusDescription";
        private const int FromBeginning = 0;
        private readonly string _pattern = @"(THIS IS A PATTERN)";
        public ConcurrentDictionary<string, string> ParseResponse(string response)
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
