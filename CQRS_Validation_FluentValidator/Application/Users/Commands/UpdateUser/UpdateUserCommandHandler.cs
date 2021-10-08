using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Messaging;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Users.Commands.UpdateUser
{
    internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            user.Update(request.FirstName, request.LastName);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
