using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Messaging;
using Application.Contracts.Users;
using Domain.Exceptions;
using Domain.Repositories;
using Mapster;

namespace Application.Users.Queries.GetUserById
{
    internal sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository) => _userRepository = userRepository;

        public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            return user.Adapt<UserResponse>();
        }
    }
}