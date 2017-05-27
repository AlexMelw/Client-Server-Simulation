namespace Protocol.Implementation.Response
{
    using System.Collections.Concurrent;
    using System.Text.RegularExpressions;
    using Interfaces.CommonConventions;
    using Interfaces.Response;

    public class ResponseParser : IFlowProtocolResponseParser
    {
        private readonly string AuthenticationResponsePattern =
                @"(?:(?<statuscode>(?:\d{3}))\s+(?<statusdesc>(?:OK|ERR))\s+(?<cmd>AUTH)\s+--RES='(?<resvalue>unauthorized|authorized)')"
            ;

        public ConcurrentDictionary<string, string> ParseResponse(string response)
        {
            Regex parser = new Regex(AuthenticationResponsePattern);
            Match match = parser.Match(response);

            if (match.Success)
            {
                var responseComponents = new ConcurrentDictionary<string, string>();
                responseComponents.TryAdd(Conventions.StatusCode, match.Groups[Conventions.StatusCode].Value);
                responseComponents.TryAdd(Conventions.StatusDesc, match.Groups[Conventions.StatusDesc].Value);
                responseComponents.TryAdd(Conventions.Cmd, match.Groups[Conventions.Cmd].Value);
                responseComponents.TryAdd(Conventions.Res, match.Groups[Conventions.Res].Value);

                return responseComponents;
            }
            return null;
        }
    }
}