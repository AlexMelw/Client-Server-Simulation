namespace FlowProtocol.Implementation.Request
{
    using System.Collections.Concurrent;
    using System.Text.RegularExpressions;
    using Interfaces.Request;
    using static Interfaces.CommonConventions.Conventions;

    public class RequestParser : IFlowProtocolRequestParser
    {
        private const string ClientTypeListenPortPattern =
            @"\s+--clienttype='(?<clienttype>(?:udp|tcp))'\s+--listenport='(?<listenport>\d{1,5})'";

        private const long Size10MB = 10 * 1024 * 1024L;
        private const string StatusCode = "statuscode";
        private const string ObjectType = "objecttype";
        private const string ObjectValue = "objectvalue";
        private const string StatusDescription = "statusdescription";
        private const int FromBeginning = 0;

        private readonly string _translatePattern =
                @"(?:(?<cmd>TRANSLATE)\s+--sourcetext='(?<sourcetext>.*)'\s+--sourcelang='(?<sourcelang>ro|ru|en|unknown)'\s+--targetlang='(?<targetlang>ro|ru|en)')"
            ;

        public ConcurrentDictionary<string, string> ParseRequest(string request)
        {
            var responseComponents = new ConcurrentDictionary<string, string>();

            Regex parser = new Regex(_translatePattern);
            Match match = parser.Match(request);

            if (match.Success)
            {
                responseComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                responseComponents.TryAdd(SourceText, match.Groups[SourceText].Value);
                responseComponents.TryAdd(SourceLang, match.Groups[SourceLang].Value);
                responseComponents.TryAdd(TargetLang, match.Groups[TargetLang].Value);

                return responseComponents;
            }

            return null;
        }
    }
}