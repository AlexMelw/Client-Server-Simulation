namespace FlowProtocol.Implementation.Request.Commands.Implementers.Unprotected
{
    using System.Diagnostics;
    using FlowProtocol.Interfaces.CommonConventions;
    using Interfaces;

    public class NotFoundRequestCommand : IRequestCommand
    {
        private readonly string _cmd;

        #region CONSTRUCTORS

        public NotFoundRequestCommand(string cmd) => _cmd = cmd;

        #endregion

        public string Execute()
        {
            Debug.WriteLine($"Request command [{_cmd}] is not found!");
            return Conventions.BadRequest;
        }
    }
}