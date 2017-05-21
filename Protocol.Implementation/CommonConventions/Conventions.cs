using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Implementation.CommonConventions
{
    public class Conventions
    {
        public const string CloseConnection = "CLOSECONNECTION";
        public const string CloseConnectionAccepted = "CLOSECONNECTIONACCEPTED";
        public const string ServerHalted = "SERVERHALTED";
        public const int TcpServerListeningPort = 6418;
        public const int UdpServerListeningPort = 8146;

        public class Lang
        {
            public const string English = "en";
            public const string Romanian = "ro";
            public const string Russian = "ru";
            public const string Unknown = "unknown";
        }

        public class CommunicationType
        {
            public const string Udp = "udp";
            public const string Tcp = "tcp";
        }
        public class TagMode
        {
            public const string Any = "any";
            public const string All = "all";
        }
    }
}