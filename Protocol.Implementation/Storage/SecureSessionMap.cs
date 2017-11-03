namespace FlowProtocol.Implementation.Storage
{
    using System;
    using System.Collections.Concurrent;

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
}