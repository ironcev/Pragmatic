using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Interaction;

namespace TinyDdd.Example.Model.Users
{
    public class DeleteUserCommandHandler : BaseCommandHandler, ICommandHandler<DeleteUserCommand, Response>
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
