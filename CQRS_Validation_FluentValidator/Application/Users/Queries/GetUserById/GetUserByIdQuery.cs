using Application.Abstractions.Messaging;
using Application.Contracts.Users;

namespace Application.Users.Queries.GetUserById
{
    public sealed record GetUserByIdQuery(int UserId) : IQuery<UserResponse>;
}
