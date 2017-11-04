namespace FlowProtocol.Implementation.ProtocolHelpers
{
    public static class Base64Util
    {
        public static string Normalize(string base64EncodedText)
        {
            base64EncodedText = base64EncodedText.Replace(" ", "+");

            int mod4 = base64EncodedText.Length % 4;

            if (mod4 > 0)
            {
                base64EncodedText += new string('=', 4 - mod4);
            }
            return base64EncodedText;
        }
    }
}