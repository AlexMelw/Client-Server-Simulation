using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Interfaces.ProtocolHelpers
{
    using EasySharp.NHelpers;

    public static class CommunicationProtocolEncodingHelper
    {
        public static byte[] ToFlowProtocolAsciiEncodedBytesArray(this string source)
        {
            return source.ToBase64String().ToAsciiEncodedByteArray();
        }

        public static string ToFlowProtocolAsciiDecodedString(this byte[] source)
        {
            return source.ToAsciiString().ToDecodedStringFromBase64();
        }

    }
}