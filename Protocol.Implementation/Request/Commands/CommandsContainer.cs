namespace FlowProtocol.Implementation.Request.Commands
{
    using System;
    using System.Collections.Concurrent;
    using FlowProtocol.Interfaces.CommonConventions;
    using Implementers.Protected;
    using Implementers.Unprotected;
    using Interfaces;

    public class CommandsContainer

    {
        public ConcurrentDictionary<string, Lazy<IRequestCommandFactory>> Commands { get; } =

            new ConcurrentDictionary<string, Lazy<IRequestCommandFactory>>
            {
                [Conventions.Commands.Hello] =
                new Lazy<IRequestCommandFactory>(() => new HelloCommand(), true),

                [Conventions.Commands.Confidential] =
                new Lazy<IRequestCommandFactory>(() => new ConfidentialCommand(), true),

                [Conventions.Commands.Auth] =
                new Lazy<IRequestCommandFactory>(() => new AuthenticationCommand(), true),

                [Conventions.Commands.Register] =
                new Lazy<IRequestCommandFactory>(() => new RegisterCommand(), true),

                [Conventions.Commands.SendMessage] =
                new Lazy<IRequestCommandFactory>(() => new SendMessageCommand(), true),

                [Conventions.Commands.GetMessage] =
                new Lazy<IRequestCommandFactory>(() => new GetMessageCommand(), true),

                [Conventions.Commands.Translate] =
                new Lazy<IRequestCommandFactory>(() => new TranslateCommand(), true),
            };
    }
}