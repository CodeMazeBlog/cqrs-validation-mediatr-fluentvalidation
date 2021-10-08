using System.Collections.Generic;
using Application.Abstractions.Messaging;
using Application.Contracts.Users;

namespace Application.Users.Queries.GetUsers
{
    public sealed record GetUsersQuery() : IQuery<List<UserResponse>>;
}
