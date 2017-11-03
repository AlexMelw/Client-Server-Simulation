namespace FlowProtocol.Implementation.Response
{
    using System;
    using System.Collections.Concurrent;
    using System.Text.RegularExpressions;
    using Interfaces.CommonConventions;
    using Interfaces.Response;
    using static Interfaces.CommonConventions.Conventions;

    public class ResponseParser : IFlowProtocolResponseParser
    {
        private const string AuthenticationResponsePattern =
                @"(?:(?:(?<statuscode>\d{3})\s+(?<statusdesc>OK)\s+(?<cmd>AUTH)\s+--res='(?<res>(?s:.+))'\s+--sessiontoken='(?<sessiontoken>(?i:[{(?:]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?))')|(?:(?<statuscode>\d{3})\s+(?<statusdesc>ERR)\s+(?<cmd>AUTH)\s+--res='(?<res>(?s:.+))'))"
            ;

        private const string GetMessageResponsePattern =
                @"(?:(?<statuscode>\d{3})\s+(?<statusdesc>ERR)\s+(?<cmd>GETMSG)\s+--res='(?<res>(?s:.+))')|(?:(?<statuscode>\d{3})\s+(?<statusdesc>OK)\s+(?<cmd>GETMSG)\s+--senderid='(?<senderid>\w+)'\s+--sendername='(?<sendername>(?s:.+))'\s+--msg='(?<message>(?s:.+))')"
            ;

        private const string RegisterResponsePattern =
            @"(?:(?<statuscode>\d{3})\s+(?<statusdesc>(?:OK|ERR))\s+(?<cmd>REGISTER)\s+--res='(?<res>(?s:.+))')";

        private const string SendMessageResponsePattern =
            @"(?:(?<statuscode>\d{3})\s+(?<statusdesc>OK|ERR)\s+(?<cmd>SENDMSG)\s+--res='(?<res>(?s:.+))')";

        private const string TranslateResponsePattern =
            @"(?:(?<statuscode>\d{3})\s+(?<statusdesc>OK)\s+(?<cmd>TRANSLATE)\s+--res='(?<res>(?s:.+))')";

        private const string ShutdownServerResponsePattern =
            @"(?:(?<statuscode>\d{3})\s+(?<statusdesc>OK)\s+(?<cmd>SHUTDOWN)\s+--res='(?<res>(?s:.+))')";

        private const string HelloResponsePattern = @"(?:(?<statuscode>200)\s+(?<statusdesc>OK)\s+(?<cmd>HELLO)\s+--pubkey='(?:(?<e>[0-9A-F]+)\|(?<m>[0-9A-F]+))'\s+--sessionkey='(?<sessionkey>(?i:[{(?:]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?))')";
        // 200 OK HELLO --pubkey='0123456789ABCDEF|0123456789ABCDEF' --sessionkey='4b6ef0fd-278d-44a9-bc1ab36d1117d7cd'

        public ConcurrentDictionary<string, string> ParseResponse(string response)
        {
            var responseComponents = new ConcurrentDictionary<string, string>();

            // <TRANSLATE> RESPONSE
            Regex parser = new Regex(TranslateResponsePattern);
            Match match = parser.Match(response);

            if (match.Success)
            {
                responseComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                responseComponents.TryAdd(StatusCode, match.Groups[StatusCode].Value);
                responseComponents.TryAdd(StatusDescription, match.Groups[StatusDescription].Value);
                responseComponents.TryAdd(ResultValue, match.Groups[ResultValue].Value);

                return responseComponents;
            }

            // <REGISTRATION> RESPONSE
            parser = new Regex(RegisterResponsePattern);
            match = parser.Match(response);

            if (match.Success)
            {
                responseComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                responseComponents.TryAdd(StatusCode, match.Groups[StatusCode].Value);
                responseComponents.TryAdd(StatusDescription, match.Groups[StatusDescription].Value);
                responseComponents.TryAdd(ResultValue, match.Groups[ResultValue].Value);

                return responseComponents;
            }

            // <AUTHENTICATION> RESPONSE
            parser = new Regex(AuthenticationResponsePattern);
            match = parser.Match(response);

            if (match.Success)
            {
                responseComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                responseComponents.TryAdd(StatusCode, match.Groups[StatusCode].Value);
                responseComponents.TryAdd(StatusDescription, match.Groups[StatusDescription].Value);
                responseComponents.TryAdd(ResultValue, match.Groups[ResultValue].Value);

                if (match.Groups[SessionToken].Success)
                {
                    responseComponents.TryAdd(SessionToken, match.Groups[SessionToken].Value);
                }

                return responseComponents;
            }

            // <SEND MESSAGE> RESPONSE
            parser = new Regex(SendMessageResponsePattern);
            match = parser.Match(response);

            if (match.Success)
            {
                responseComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                responseComponents.TryAdd(StatusCode, match.Groups[StatusCode].Value);
                responseComponents.TryAdd(StatusDescription, match.Groups[StatusDescription].Value);
                responseComponents.TryAdd(ResultValue, match.Groups[ResultValue].Value);

                return responseComponents;
            }

            // <GET MESSAGE> RESPONSE
            parser = new Regex(GetMessageResponsePattern);
            match = parser.Match(response);

            if (match.Success)
            {
                responseComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                responseComponents.TryAdd(StatusCode, match.Groups[StatusCode].Value);
                responseComponents.TryAdd(StatusDescription, match.Groups[StatusDescription].Value);

                if (match.Groups[ResultValue].Success)
                {
                    responseComponents.TryAdd(ResultValue, match.Groups[ResultValue].Value);
                }
                else
                {
                    responseComponents.TryAdd(SenderName, match.Groups[SenderName].Value);
                    responseComponents.TryAdd(SenderId, match.Groups[SenderId].Value);
                    responseComponents.TryAdd(Message, match.Groups[Message].Value);
                }

                return responseComponents;
            }

            // <SHUTDOWN SERVER> RESPONSE
            parser = new Regex(ShutdownServerResponsePattern);
            match = parser.Match(response);

            if (match.Success)
            {
                responseComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                responseComponents.TryAdd(StatusCode, match.Groups[StatusCode].Value);
                responseComponents.TryAdd(StatusDescription, match.Groups[StatusDescription].Value);
                responseComponents.TryAdd(ResultValue, match.Groups[ResultValue].Value);

                return responseComponents;
            }

            // <HELLO> RESPONSE
            parser = new Regex(HelloResponsePattern);
            match = parser.Match(response);

            if (match.Success)
            {
                responseComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                responseComponents.TryAdd(StatusCode, match.Groups[StatusCode].Value);
                responseComponents.TryAdd(StatusDescription, match.Groups[StatusDescription].Value);
                responseComponents.TryAdd(Exponent, match.Groups[Exponent].Value);
                responseComponents.TryAdd(Modulus, match.Groups[Modulus].Value);
                responseComponents.TryAdd(SessionKey, match.Groups[SessionKey].Value);

                return responseComponents;
            }

            return null;
        }
    }
}