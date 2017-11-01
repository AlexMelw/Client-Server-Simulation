namespace FlowProtocol.Interfaces.CommonConventions
{
    public class Conventions
    {
        public const int TcpServerListeningPort = 46418;
        public const int UdpServerListeningPort = 48146;
        public const int EthernetTcpUdpPacketSize = 1472;
        public const int FromBeginning = 0;

        public const string Error = "ERR";
        public const string Ok = "OK";
        public const string Cmd = "cmd";
        public const string ResultValue = "res";
        public const string StatusCode = "statuscode";
        public const string StatusDescription = "statusdesc";
        public const string CloseConnection = "CLOSE_CONNECTION";
        public const string ConnectionClosed = "CONNECTION_CLOSED";
        public const string ServerHalted = "SERVER_HALTED";
        public const string Localhost = "127.0.0.1";
        public const string QuitServerCmd = "QUIT_SERVER";
        public const string Quit = "QUIT";
        public const string NotAuthenticated = "NOT_AUTHENTICATED";
        public const string BadRequest = "400 Bad Request UNKNOWN --res='Not applicable'";

        // GET MESSAGE
        public const string TranslationMode = "translationmode";

        public const string DoNotTranslate = "donottranslate";
        public const string DoTranslate = "translateto";
        public const string SenderName = "sendername";
        public const string SenderId = "senderid";


        // SEND MESSAGE
        public const string Sender = "sender";

        public const string Recipient = "recipient";
        public const string Message = "message";
        public const string SessionToken = "sessiontoken";

        // REGISTER
        public const string Name = "name";

        public const string Pass = "pass";
        public const string Login = "login";

        // AUTHENTICATION
        public const string Exponent = "e";
        public const string Modulus = "m";

        // TRANSLATE
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

        public class ClientType
        {
            public const string Udp = "UDP";
            public const string Tcp = "TCP";
        }

        public class TagMode
        {
            public const string Any = "any";
            public const string All = "all";
        }

        public class Commands
        {
            public const string GetMessage = "GETMSG";
            public const string SendMessage = "SENDMSG";
            public const string Translate = "TRANSLATE";
            public const string Register = "REGISTER";
            public const string Auth = "AUTH";
            public const string Hello = "HELLO";
        }
    }
}