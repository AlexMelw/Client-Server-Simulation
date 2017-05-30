namespace FlowProtocol.Implementation.Response
{
    using System.Collections.Concurrent;
    using System.Text.RegularExpressions;
    using Interfaces.Response;
    using static Interfaces.CommonConventions.Conventions;

    public class ResponseParser : IFlowProtocolResponseParser
    {
        private readonly string _authenticationResponsePattern =
                @"(?:(?:(?<statuscode>\d{3})\s+(?<statusdesc>OK)\s+(?<cmd>AUTH)\s+--res='(?<res>.+)'\s+sessiontoken='(?<sessiontoken>(?i:[{(?:]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?))')|(?:(?<statuscode>\d{3})\s+(?<statusdesc>ERR)\s+(?<cmd>AUTH)\s+--res='(?<res>.+)'))"
            ;

        private readonly string _getMessageResponsePattern =
                @"(?:(?<statuscode>\d{3})\s+(?<statusdesc>ERR)\s+(?<cmd>GETMSG)\s+--res='(?<res>.+)')|(?:(?<statuscode>\d{3})\s+(?<statusdesc>OK)\s+(?<cmd>GETMSG)\s+--senderid='(?<senderid>\w+)'\s+--sendername='(?<sendername>.+)'\s+--msg='(?<message>.+)')"
            ;

        private readonly string _registerResponsePattern =
            @"(?:(?<statuscode>\d{3})\s+(?<statusdesc>(?:OK|ERR))\s+(?<cmd>REGISTER)\s+--RES='(?<res>.+)')";

        private readonly string _sendMessageResponsePattern =
            @"(?:(?<statuscode>\d{3})\s+(?<statusdesc>OK|ERR)\s+(?<cmd>SENDMSG)\s+--res='(?<res>.+)')";

        private readonly string _translateResponsePattern =
            @"(?:(?<statuscode>200)\s+(?<statusdesc>OK)\s+(?<cmd>TRANSLATE)\s+--RES='(?<res>.+)')";

        private readonly string _shutdownServerResponsePattern =
            @"(?:(?<statuscode>200)\s+(?<statusdesc>OK)\s+(?<cmd>SHUTDOWN)\s+--res='(?<res>.+)')";

        public ConcurrentDictionary<string, string> ParseResponse(string response)
        {
            var responseComponents = new ConcurrentDictionary<string, string>();

            // <TRANSLATE> RESPONSE
            Regex parser = new Regex(_translateResponsePattern);
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
            parser = new Regex(_registerResponsePattern);
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
            parser = new Regex(_authenticationResponsePattern);
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
            parser = new Regex(_sendMessageResponsePattern);
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
            parser = new Regex(_getMessageResponsePattern);
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
            parser = new Regex(_shutdownServerResponsePattern);
            match = parser.Match(response);

            if (match.Success)
            {
                responseComponents.TryAdd(Cmd, match.Groups[Cmd].Value);
                responseComponents.TryAdd(StatusCode, match.Groups[StatusCode].Value);
                responseComponents.TryAdd(StatusDescription, match.Groups[StatusDescription].Value);
                responseComponents.TryAdd(ResultValue, match.Groups[ResultValue].Value);

                return responseComponents;
            }

            return null;
        }
    }
}