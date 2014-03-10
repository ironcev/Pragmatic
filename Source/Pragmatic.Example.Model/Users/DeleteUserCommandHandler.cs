using Pragmatic.Interaction;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.Model.Users
{
    public sealed class DeleteUserCommandHandler : BaseCommandHandler, ICommandHandler<DeleteUserCommand, Response>
    {
        public DeleteUserCommandHandler(QueryExecutor queryExecutor, UnitOfWork unitOfWork) : base(queryExecutor, unitOfWork)
        {
        }

        public Response Execute(DeleteUserCommand command)
        {
            Argument.IsNotNull(command, "command");

            UnitOfWork.Begin();
            UnitOfWork.RegisterEntityToDelete(command.User);
            UnitOfWork.Commit();

            return new Response();
        }
    }
}
