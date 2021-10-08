using FluentValidation;

namespace Application.Users.Commands.UpdateUser
{
public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);

        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
    }
}
}
