namespace FlowProtocol.Implementation.ProtocolHelpers
{
    using System;
    using EasySharp.NHelpers.CustomExMethods;

    public static class FlowProtocolEncodingHelper
    {
        public static byte[] ToFlowProtocolAsciiEncodedBytesArray(this string source)
        {
            return source.ToBase64String().ToAsciiEncodedByteArray();
        }

        public static string ToFlowProtocolAsciiDecodedString(this byte[] source)
        {
            return source.ToAsciiString().ToDecodedStringFromBase64();
        }

        public static string ToBase64String(this byte[] source)
        {
            return Convert.ToBase64String(source);
        }

        public static byte[] FromBase64StringToByteArray(this string source)
        {
            return Convert.FromBase64String(source);
        }
    }
}