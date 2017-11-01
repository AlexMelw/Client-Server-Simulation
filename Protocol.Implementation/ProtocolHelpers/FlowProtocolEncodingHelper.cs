namespace FlowProtocol.Implementation.ProtocolHelpers
{
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
    }
}