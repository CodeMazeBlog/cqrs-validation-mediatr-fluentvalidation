using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Messaging;
using Application.Contracts.Users;
using Domain.Entities;
using Domain.Repositories;
using Mapster;

namespace Application.Users.Commands.CreateUser
{
    internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(request.FirstName, request.LastName);

            _userRepository.Insert(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Adapt<UserResponse>();
        }
    }
}