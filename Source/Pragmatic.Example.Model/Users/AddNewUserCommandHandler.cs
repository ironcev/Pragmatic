using FluentValidation;
using Pragmatic.FluentValidation;
using Pragmatic.Interaction;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.Model.Users
{
    public sealed class AddNewUserCommandHandler : BaseInteractionHandler, ICommandHandler<AddNewUserCommand, Response<User>> // TODO-IG: Do we really want the handlers to be public? They are now public just because of the StructureMap.
    {
        private readonly IValidator<User> _userValidator;

        public AddNewUserCommandHandler(QueryExecutor queryExecutor, UnitOfWork unitOfWork, IValidator<User> userValidator)
            : base(queryExecutor, unitOfWork)
        {
            Argument.IsNotNull(userValidator, "userValidator");

            _userValidator = userValidator;
        }

        public Response<User> Execute(AddNewUserCommand command)
        {
            Argument.IsNotNull(command, "command");

            Response response = new Response();

            User newUser = MapCommandToUser(command);

            response.AddErrors(_userValidator.Validate(newUser));
            if (response.HasErrors) return Response<User>.From(response);

            var userWithTheSameEmail = QueryExecutor.GetOne<User>(user => user.Email == newUser.Email);

            if (userWithTheSameEmail.IsSome)
                return Response<User>.From(response.AddError("User with the same email already exists."));

            UnitOfWork.Begin();
            UnitOfWork.RegisterEntityToAddOrUpdate(newUser);
            UnitOfWork.Commit();

            return Response<User>.From(response, newUser);
        }

        private static User MapCommandToUser(AddNewUserCommand command) // TODO-IG: Remove this and use Automapper.
        {
            System.Diagnostics.Debug.Assert(command != null);

            return new User
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email
            };
        }
    }
}
