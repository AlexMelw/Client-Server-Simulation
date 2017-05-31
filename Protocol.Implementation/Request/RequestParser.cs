namespace FlowProtocol.Implementation.Request
{
    using System.Collections.Concurrent;
    using System.Text.RegularExpressions;
    using Interfaces.Request;
    using static Interfaces.CommonConventions.Conventions;

    public class RequestParser : IFlowProtocolRequestParser
    {
        private readonly string _authenticationRequestPattern =
                @"(?:(?<cmd>AUTH)\s+(\s+securepassword\s+protectiontype='(?<passprotectiontype>\w+)')?\s+--login='(?<login>\w+)'\s+--pass='(?<pass>.+)')"
            ;

        private readonly string _getMessageRequestPattern =
                @"(?:(?<cmd>GETMSG)\s+--sessiontoken='(?<sessiontoken>(?i:[{(?:]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?))'\s+(?:--(?<translationmode>donottranslate)|(?:--(?<translationmode>translateto)='(?<targetlang>en|ro|ru)')))"
            ;

        private readonly string _registerRequestPattern =
            @"(?:(?<cmd>REGISTER)\s+--login='(?<login>[\w]+)'\s+--pass='(?<pass>.+)'\s+--name='(?<name>(?:\w|\s)+)')";

        private readonly string _sendMessageRequestPattern =
                @"(?:(?<cmd>SENDMSG)\s+--to='(?<recipient>\w+)'\s+--msg='(?<message>.*)'\s+--sourcelang='(?<sourcelang>en|ro|ru|unknown)'\s+--sessiontoken='(?<sessiontoken>(?i:[{(?:]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?))')"
            ;

        private readonly string _translateRequestPattern =
                @"(?:(?<cmd>TRANSLATE)\s+--sourcetext='(?<sourcetext>.*)'\s+--sourcelang='(?<sourcelang>ro|ru|en|unknown)'\s+--targetlang='(?<targetlang>ro|ru|en)')"
            ;

        private readonly string _helloRequestPattern = @"(?<cmd>HELLO)";

        public ConcurrentDictionary<string, string> ParseRequest(string request)
        {
            var requestComponents = new ConcurrentDictionary<string, string>();

            // <HELLO> REQUEST
            Regex parser = new Regex(_helloRequestPattern);
            Match match = parser.Match(request);

            if (match.Success)
            {
                requestComponents.TryAdd(Cmd, match.Groups[Cmd].Value);

                return requestComponents;
            }

            // <REGISTER> REQUEST
            parser = new Regex(_registerRequestPattern);
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
            parser = new Regex(_authenticationRequestPattern);
            match = parser.Match(request);

            if (match.Success)
            {
                requestComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                requestComponents.TryAdd(Login, match.Groups[Login].Value);
                requestComponents.TryAdd(Pass, match.Groups[Pass].Value);

                return requestComponents;
            }

            // <SEND MESSAGE> REQUEST
            parser = new Regex(_sendMessageRequestPattern);
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
            parser = new Regex(_getMessageRequestPattern);
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
            parser = new Regex(_translateRequestPattern);
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