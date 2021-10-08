using Application.Abstractions.Messaging;
using MediatR;

namespace Application.Users.Commands.UpdateUser
{
    public sealed record UpdateUserCommand(int UserId, string FirstName, string LastName) : ICommand<Unit>;
}
