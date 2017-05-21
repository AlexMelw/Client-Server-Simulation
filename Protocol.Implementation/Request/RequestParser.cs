namespace Protocol.Implementation.Request
{
    using System.Collections.Concurrent;
    using System.Text.RegularExpressions;
    using Interfaces;
    using Interfaces.Request;

    public class RequestParser : ICommunicationProtocolRequestParser
    {
        private const string ClientTypeListenPortPattern =
            @"\s+--clienttype='(?<clienttype>(?:udp|tcp))'\s+--listenport='(?<listenport>\d{1,5})'";

        private const string ClientType = "clienttype";
        private const string ListenPort = "listenport";
        private const string SourceText = "sourcetext";
        private const string SourceLang = "sourcelang";
        private const string TargetLang = "targetlang";

        private const long Size10MB = 10 * 1024 * 1024L;
        private const string StatusCode = "StatusCode";
        private const string ObjectType = "objectType";
        private const string ObjectValue = "objectValue";
        private const string StatusDescription = "StatusDescription";
        private const int FromBeginning = 0;

        private readonly string AuthenticationPattern =
                @"(?:(?<cmd>AUTH)\s+--clienttype='(?<clienttype>(?:udp|tcp))'\s+--listenport='(?<listenport>\d{1,5})'(\s+securepassword\s+protectiontype='(?<passprotectiontype>\w+)')?\s+--login='(?<login>\w+)'\s+--pass='(?<pass>.+)')"
            ;

        private readonly string GimmeImgPattern =
                @"(?:(?<cmd>GIMMEIMG)\s+--clienttype='(?<clienttype>(?:udp|tcp))'\s+--listenport='(?<listenport>\d{1,5})'\s+--tags='(?<tags>((\w+,)*\w+))'\s+--tagmode='(?<tagmode>any|all)'\s+--lang='(?<lang>en|ro|ru|unknown)'\s+--format='json')"
            ;

        private readonly string RegistrationPattern =
                @"(?:(?<cmd>REGISTER)\s+--clienttype='(?<clienttype>(?:udp|tcp))'\s+--listenport='(?<listenport>\d{1,5})'\s+--login='(?<login>[\w]+)'\s+--pass='(?<pass>.+)'\s+--name='(?<name>(?:\w|\s)+)')"
            ;

        private readonly string SendMessagePattern =
                @"(?:(?<cmd>SENDMSG)\s+--clienttype='(?<clienttype>(?:udp|tcp))'\s+--listenport='(?<listenport>\d{1,5})'\s+--from='(?<sender>\w+)'\s+--to='(?<recipient>\w+)'\s+--message='(?<message>.*)'\s+--sourcelang='(?<sourcelang>en|ro|ru|unknown)')"
            ;

        private readonly string TranslateCmdPattern =
                $@"(?:(?<cmd>TRANSLATE){
                        ClientTypeListenPortPattern
                    }\s+--sourcetext='(?<sourcetext>.*)'\s+--sourcelang='(?<sourcelang>ro|ru|en|unknown)'\s+--targetlang='(?<targetlang>ro|ru|en)')"
            ;

        public ConcurrentDictionary<string, string> ParseResponse(string response)
        {
            var responseComponents = new ConcurrentDictionary<string, string>();

            Regex parser = new Regex(TranslateCmdPattern);
            Match match = parser.Match(response);

            if (match.Success)
            {
                responseComponents.TryAdd(ClientType, match.Groups[ClientType].Value);
                responseComponents.TryAdd(ListenPort, match.Groups[ListenPort].Value);
                responseComponents.TryAdd(SourceText, match.Groups[SourceText].Value);
                responseComponents.TryAdd(SourceLang, match.Groups[SourceLang].Value);
                responseComponents.TryAdd(TargetLang, match.Groups[TargetLang].Value);

                return responseComponents;
            }

            return null;
        }
    }
}