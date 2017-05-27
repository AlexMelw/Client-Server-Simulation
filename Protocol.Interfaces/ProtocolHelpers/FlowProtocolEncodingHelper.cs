namespace Protocol.Interfaces.ProtocolHelpers
{
    using EasySharp.NHelpers;

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
    }
}