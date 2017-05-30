namespace FlowProtocol.Implementation.Storage
{
    using System;
    using System.Collections.Concurrent;
    using Entities;

    public sealed class RegisteredUsers
    {
        private static readonly Lazy<RegisteredUsers> Lazy =
            new Lazy<RegisteredUsers>(() => new RegisteredUsers(), true);

        /// <summary>
        ///     Search key is User's login
        /// </summary>
        public readonly ConcurrentDictionary<string, User> Users;

        public static RegisteredUsers Instance => Lazy.Value;

        #region CONSTRUCTORS

        private RegisteredUsers()
        {
            Users = new ConcurrentDictionary<string, User>();
        }

        #endregion
    }
}