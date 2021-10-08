using Application.Abstractions.Messaging;
using Application.Contracts.Users;

namespace Application.Users.Commands.CreateUser
{
    public sealed record CreateUserCommand(string FirstName, string LastName) : ICommand<UserResponse>;
}
