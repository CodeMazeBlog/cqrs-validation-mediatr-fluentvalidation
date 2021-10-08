using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Messaging;
using Application.Contracts.Users;
using Domain.Repositories;
using Mapster;

namespace Application.Users.Queries.GetUsers
{
    internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, List<UserResponse>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository) => _userRepository = userRepository;

        public async Task<List<UserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAsync(cancellationToken);

            return users.Adapt<List<UserResponse>>();
        }
    }
}