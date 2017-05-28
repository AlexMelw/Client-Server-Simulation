namespace FlowProtocol.Implementation.Entities
{
    using System;

    public class AuthClient
    {
        public User User { get; set; }
        public Guid AuthToken { get; set; }

        public static AuthClient Empty => new AuthClient();
    }
}