namespace FlowProtocol.Implementation.Storage
{
    using System.Security.Cryptography;

    public sealed class Keys
    {
        public RSAParameters RemotePublicKey { get; set; }
        public RSAParameters ServerPublicKey { get; set; }
        public RSAParameters ServerPrivateKey { get; set; }
    }
}