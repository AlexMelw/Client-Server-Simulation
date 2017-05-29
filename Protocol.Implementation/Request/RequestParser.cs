namespace FlowProtocol.Implementation.Request
{
    using System.Collections.Concurrent;
    using System.Text.RegularExpressions;
    using Interfaces.Request;
    using static Interfaces.CommonConventions.Conventions;

    public class RequestParser : IFlowProtocolRequestParser
    {
        private readonly string _registerPattern =
            @"(?:(?<cmd>REGISTER)\s+--login='(?<login>[\w]+)'\s+--pass='(?<pass>.+)'\s+--name='(?<name>(?:\w|\s)+)')";

        private readonly string _authenticationPattern =
                @"(?:(?<cmd>AUTH)\s+(\s+securepassword\s+protectiontype='(?<passprotectiontype>\w+)')?\s+--login='(?<login>\w+)'\s+--pass='(?<pass>.+)')"
            ;

        private readonly string _sendMessagePattern =
                @"(?:(?<cmd>SENDMSG)\s+--to='(?<recipient>\w+)'\s+--msg='(?<message>.*)'\s+--sourcelang='(?<sourcelang>en|ro|ru|unknown)'\s+sessiontoken='(?<sessiontoken>[{(?:]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?)')"
            ;

        private readonly string _getMessagePattern =
                @"(?:(?<cmd>GETMSG)\s+sessiontoken='(?<sessiontoken>[{(?:]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?)'\s+(?:(?<translationmode>donottranslate)|(?:(?<translationmode>translateto)='(?<targetlang>en|ro|ru)')))"
            ;

        // TODO To be reviewed (add sessiontoken)
        private readonly string _translatePattern =
                @"(?:(?<cmd>TRANSLATE)\s+--sourcetext='(?<sourcetext>.*)'\s+--sourcelang='(?<sourcelang>ro|ru|en|unknown)'\s+--targetlang='(?<targetlang>ro|ru|en)')"
            ;

        public ConcurrentDictionary<string, string> ParseRequest(string request)
        {
            var responseComponents = new ConcurrentDictionary<string, string>();

            // <REGISTER> REQUEST
            Regex parser = new Regex(_registerPattern);
            Match match = parser.Match(request);

            if (match.Success)
            {
                responseComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                responseComponents.TryAdd(Login, match.Groups[Login].Value);
                responseComponents.TryAdd(Pass, match.Groups[Pass].Value);
                responseComponents.TryAdd(Name, match.Groups[Name].Value);

                return responseComponents;
            }

            // <AUTHENTICATION> REQUEST
            parser = new Regex(_authenticationPattern);
            match = parser.Match(request);

            if (match.Success)
            {
                responseComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                responseComponents.TryAdd(Login, match.Groups[Login].Value);
                responseComponents.TryAdd(Pass, match.Groups[Pass].Value);

                return responseComponents;
            }

            // <SEND MESSAGE> REQUEST
            parser = new Regex(_sendMessagePattern);
            match = parser.Match(request);

            if (match.Success)
            {
                responseComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                responseComponents.TryAdd(SessionToken, match.Groups[SessionToken].Value);
                responseComponents.TryAdd(Recipient, match.Groups[Recipient].Value);
                responseComponents.TryAdd(Message, match.Groups[Message].Value);
                responseComponents.TryAdd(SourceLang, match.Groups[SourceLang].Value);

                return responseComponents;
            }

            // <GET MESSAGE> REQUEST
            parser = new Regex(_getMessagePattern);
            match = parser.Match(request);

            if (match.Success)
            {
                responseComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                responseComponents.TryAdd(SessionToken, match.Groups[SessionToken].Value);
                responseComponents.TryAdd(TranslationMode, match.Groups[TranslationMode].Value);
                responseComponents.TryAdd(TargetLang, match.Groups[TargetLang].Value);

                return responseComponents;
            }

            // <TRANSLATE> REQUEST
            parser = new Regex(_translatePattern);
            match = parser.Match(request);

            // TODO To be reviewed (add sessiontoken)
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