namespace FlowProtocol.Implementation.Request
{
    using System.Collections.Concurrent;
    using System.Text.RegularExpressions;
    using Interfaces.Request;
    using static Interfaces.CommonConventions.Conventions;

    public class RequestParser : IFlowProtocolRequestParser
    {
        private const string HelloRequestPattern = @"(?<cmd>HELLO)\s+--pubkey='(?:(?<e>[0-9A-F]+)\|(?<m>[0-9A-F]+))'"; //  HELLO --pubkey='0123456789ABCDEF|0123456789ABCDEF'

        private const string EncryptedMessagePattern =
                @"(?:(?<cmd>CONF)\s+sessionkey:(?<sessionkey>(?i:[{(?:]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?))\s+secret:(?<secret>(?i:[a-z0-9\+\/\=]+)))"
            ;

        private const string AuthenticationRequestPattern =
                @"(?:(?<cmd>AUTH)\s+--login='(?<login>\w+)'\s+--pass='(?<pass>(?s:.+))')"
            ;

        private const string GetMessageRequestPattern =
                @"(?:(?<cmd>GETMSG)\s+--sessiontoken='(?<sessiontoken>(?i:[{(?:]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?))'\s+(?:--(?<translationmode>donottranslate)|(?:--(?<translationmode>translateto)='(?<targetlang>en|ro|ru)')))"
            ;

        private const string RegisterRequestPattern =
            @"(?:(?<cmd>REGISTER)\s+--login='(?<login>[\w]+)'\s+--pass='(?<pass>(?s:.+))'\s+--name='(?<name>(?:\w|\s)+)')";

        private const string SendMessageRequestPattern =
                @"(?:(?<cmd>SENDMSG)\s+--to='(?<recipient>\w+)'\s+--msg='(?<message>(?s:.+))'\s+--sourcelang='(?<sourcelang>en|ro|ru|unknown)'\s+--sessiontoken='(?<sessiontoken>(?i:[{(?:]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?))')"
            ;

        private const string TranslateRequestPattern =
                @"(?:(?<cmd>TRANSLATE)\s+--sourcetext='(?<sourcetext>(?s:.+))'\s+--sourcelang='(?<sourcelang>ro|ru|en|unknown)'\s+--targetlang='(?<targetlang>ro|ru|en)')"
            ;

        public ConcurrentDictionary<string, string> ParseRequest(string request)
        {
            var requestComponents = new ConcurrentDictionary<string, string>();

            // <HELLO> REQUEST
            Regex parser = new Regex(HelloRequestPattern);
            Match match = parser.Match(request);

            if (match.Success)
            {
                requestComponents.TryAdd(Cmd, match.Groups[Cmd].Value);

                return requestComponents;
            }

            // <RECEIVE ENCRYPTED MESSAGE>
            parser = new Regex(EncryptedMessagePattern);
            match = parser.Match(request);

            if (match.Success)
            {
                requestComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                requestComponents.TryAdd(SessionKey, match.Groups[SessionKey].Value);
                requestComponents.TryAdd(Secret, match.Groups[Secret].Value);

                return requestComponents;
            }

            // <REGISTER> REQUEST
            parser = new Regex(RegisterRequestPattern);
            match = parser.Match(request);

            if (match.Success)
            {
                requestComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                requestComponents.TryAdd(Login, match.Groups[Login].Value);
                requestComponents.TryAdd(Pass, match.Groups[Pass].Value);
                requestComponents.TryAdd(Name, match.Groups[Name].Value);

                return requestComponents;
            }

            // <AUTHENTICATION> REQUEST
            parser = new Regex(AuthenticationRequestPattern);
            match = parser.Match(request);

            if (match.Success)
            {
                requestComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                requestComponents.TryAdd(Login, match.Groups[Login].Value);
                requestComponents.TryAdd(Pass, match.Groups[Pass].Value);

                return requestComponents;
            }

            // <SEND MESSAGE> REQUEST
            parser = new Regex(SendMessageRequestPattern);
            match = parser.Match(request);

            if (match.Success)
            {
                requestComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                requestComponents.TryAdd(SessionToken, match.Groups[SessionToken].Value);
                requestComponents.TryAdd(Recipient, match.Groups[Recipient].Value);
                requestComponents.TryAdd(Message, match.Groups[Message].Value);
                requestComponents.TryAdd(SourceLang, match.Groups[SourceLang].Value);

                return requestComponents;
            }

            // <GET MESSAGE> REQUEST
            parser = new Regex(GetMessageRequestPattern);
            match = parser.Match(request);

            if (match.Success)
            {
                requestComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                requestComponents.TryAdd(SessionToken, match.Groups[SessionToken].Value);
                requestComponents.TryAdd(TranslationMode, match.Groups[TranslationMode].Value);
                requestComponents.TryAdd(TargetLang, match.Groups[TargetLang].Value);

                return requestComponents;
            }

            // <TRANSLATE> REQUEST
            parser = new Regex(TranslateRequestPattern);
            match = parser.Match(request);

            if (match.Success)
            {
                requestComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                requestComponents.TryAdd(SourceText, match.Groups[SourceText].Value);
                requestComponents.TryAdd(SourceLang, match.Groups[SourceLang].Value);
                requestComponents.TryAdd(TargetLang, match.Groups[TargetLang].Value);

                return requestComponents;
            }

            return null;
        }
    }
}