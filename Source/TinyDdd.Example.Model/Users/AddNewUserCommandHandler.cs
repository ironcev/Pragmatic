using FluentValidation;
using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Interaction;
using TinyDdd.FluentValidation;

namespace TinyDdd.Example.Model.Users
{
    public class AddNewUserCommandHandler : BaseCommandHandler, ICommandHandler<AddNewUserCommand, Response<User>> // TODO-IG: Do we really want the handlers to be public? They are now public just because of the StructureMap.
    {
        private readonly IValidator<User> _userValidator;

        public AddNewUserCommandHandler(QueryExecutor queryExecutor, IUnitOfWork unitOfWork, IValidator<User> userValidator)
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

            // Check if the user with the same email already exists.
            var userWithTheSameEmail = QueryExecutor.GetOne<User>(user => user.Email == newUser.Email);

            if (userWithTheSameEmail.IsSome)
                return Response<User>.From(response.AddError("User with the same email already exists.")); // TODO-IG: Switch to the new AddError() method that works with resources once this is merged to the master branch.

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
