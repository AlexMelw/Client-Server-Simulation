namespace FlowProtocol.Implementation.Storage
{
    using System;
    using System.Collections.Concurrent;
    using System.Security.Cryptography;

    public sealed class SecureSessionMap
    {
        private static readonly Lazy<SecureSessionMap> Lazy =
            new Lazy<SecureSessionMap>(() => new SecureSessionMap(), true);

        public readonly ConcurrentDictionary<Guid, Keys> Keeper;

        public static SecureSessionMap Instance => Lazy.Value;

        #region CONSTRUCTORS

        private SecureSessionMap()
        {
            Keeper = new ConcurrentDictionary<Guid, Keys>();
        }

        #endregion
    }

    public sealed class Keys
    {
        public RSAParameters RemotePublicKey { get; set; }
        public RSAParameters ServerPublicKey { get; set; }
        public RSAParameters ServerPrivateKey { get; set; }
    }
}