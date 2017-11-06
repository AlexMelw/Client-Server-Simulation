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
        public ConcurrentDictionary<string, Lazy<IFactoryRequestCommand>> Commands { get; } =

            new ConcurrentDictionary<string, Lazy<IFactoryRequestCommand>>
            {
                [Conventions.Commands.Hello] =
                new Lazy<IFactoryRequestCommand>(() => new HelloCommand(), true),

                [Conventions.Commands.Confidential] =
                new Lazy<IFactoryRequestCommand>(() => new ConfidentialCommand(), true),

                [Conventions.Commands.Auth] =
                new Lazy<IFactoryRequestCommand>(() => new AuthenticationCommand(), true),

                [Conventions.Commands.Register] =
                new Lazy<IFactoryRequestCommand>(() => new RegisterCommand(), true),

                [Conventions.Commands.SendMessage] =
                new Lazy<IFactoryRequestCommand>(() => new SendMessageCommand(), true),

                [Conventions.Commands.GetMessage] =
                new Lazy<IFactoryRequestCommand>(() => new GetMessageCommand(), true),

                [Conventions.Commands.Translate] =
                new Lazy<IFactoryRequestCommand>(() => new TranslateCommand(), true),
            };
    }
}