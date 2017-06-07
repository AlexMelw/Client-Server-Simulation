namespace FlowProtocol.Implementation.Storage
{
    using System;
    using System.Collections.Concurrent;
    using DomainModels.Entities;

    public sealed class RegisteredUsers
    {
        private static readonly Lazy<RegisteredUsers> Lazy =
            new Lazy<RegisteredUsers>(() => new RegisteredUsers(), isThreadSafe: true);

        /// <summary>
        ///     Search key is User's login
        /// </summary>
        public readonly ConcurrentDictionary<string, User> Users;

        public static RegisteredUsers Instance => Lazy.Value;


        public bool TryRegisterUser(User user)
        {
            if (Users.ContainsKey(user.Login))
            {
                return false;
            }
            return Users.TryAdd(user.Login, user);
        }

        #region CONSTRUCTORS

        private RegisteredUsers()
        {
            Users = new ConcurrentDictionary<string, User>();
        }

        #endregion
    }
}