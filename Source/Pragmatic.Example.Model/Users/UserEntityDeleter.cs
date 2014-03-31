using Pragmatic.Example.Model.Localization;
using Pragmatic.Interaction;
using Pragmatic.Interaction.EntityDeletion;
using SwissKnife;

namespace Pragmatic.Example.Model.Users
{
    public class UserEntityDeleter : EntityDeleter<User>
    {
        public UserEntityDeleter(UnitOfWork unitOfWork, QueryExecutor queryExecutor) : base(unitOfWork, queryExecutor)
        {
        }

        protected override Response<Option<User>> CanDeleteEntityCore(User entity)
        {
            Response response = new Response();

            response.AddWarning(() => UserResources.DeletingUserPermanentlyDeletesAllItsData);
            response.AddInformation(string.Format(UserResources.InformationUserFullName, entity.FullName));

            if (entity.IsAdministrator)
            {
                response.AddError(() => UserResources.AdministratorCannotBeDeleted);
                return Response<Option<User>>.From(response);
            }

            return Response<Option<User>>.From(response, entity);
        }
    }
}
