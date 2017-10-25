namespace FlowProtocol.Implementation.Storage
{
    using System;
    using System.Collections.Concurrent;
    using DomainModels.Entities;

    public sealed class AuthenticatedClients
    {
        private static readonly Lazy<AuthenticatedClients> Lazy =
            new Lazy<AuthenticatedClients>(() => new AuthenticatedClients(), true);

        /// <summary>
        ///     Search key is AuthenticatedUser's login
        /// </summary>
        public readonly ConcurrentDictionary<string, AuthClient> Clients;

        public static AuthenticatedClients Instance => Lazy.Value;

        #region CONSTRUCTORS

        private AuthenticatedClients()
        {
            Clients = new ConcurrentDictionary<string, AuthClient>();
        }

        #endregion
    }
}