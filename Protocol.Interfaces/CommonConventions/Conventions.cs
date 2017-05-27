namespace FlowProtocol.Interfaces.CommonConventions
{
    public class Conventions
    {
        public const string OK = "OK";
        public const string StatusCode = "statuscode";
        public const string Cmd = "cmd";
        public const string Res = "resvalue";
        public const string StatusDesc = "statusdesc";
        public const string CloseConnection = "CLOSE_CONNECTION";
        public const string ConnectionClosed = "CONNECTION_CLOSED";
        public const string ServerHalted = "SERVER_HALTED";
        public const string Localhost = "127.0.0.1";
        public const int TcpServerListeningPort = 46418;
        public const int UdpServerListeningPort = 48146;
        public const int FromBeginning = 0;
        public const int EthernetTcpUdpPacketSize = 1472;
        public const string QuitServerCmd = "QUIT_SERVER";
        public const string Quit = "QUIT";
        public const string Error = "ERROR";
        public const string NotAuthenticated = "NOT_AUTHENTICATED";
        public const string BadRequest = "400 Bad Request UNKNOWN --RES='Not applicable'";

        public const string SourceText = "sourcetext";
        public const string SourceLang = "sourcelang";
        public const string TargetLang = "targetlang";

        public class Lang
        {
            public const string English = "en";
            public const string Romanian = "ro";
            public const string Russian = "ru";
            public const string Unknown = "unknown";
        }

        public class TransportType
        {
            public const string Udp = "udp";
            public const string Tcp = "tcp";
        }

        public class TagMode
        {
            public const string Any = "any";
            public const string All = "all";
        }

        public class Commands
        {
            public const string Translate = "TRANSLATE";
            public const string Register = "REGISTER";
            public const string Auth = "AUTH";
        }
    }
}