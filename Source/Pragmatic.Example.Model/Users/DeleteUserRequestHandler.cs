using Pragmatic.Example.Model.Localization;
using Pragmatic.Interaction;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.Model.Users
{
    public sealed class DeleteUserRequestHandler : BaseInteractionHandler, IRequestHandler<DeleteUserRequest, Response>
    {
        public DeleteUserRequestHandler(QueryExecutor queryExecutor, UnitOfWork unitOfWork) : base(queryExecutor, unitOfWork)
        {
        }

        public Response Execute(DeleteUserRequest request)
        {
            Argument.IsNotNull(request, "request");

            return new Response().AddWarning(() => UserResources.DeletingUserPermanentlyDeletesAllItsData);
        }
    }
}
