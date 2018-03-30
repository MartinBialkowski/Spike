using FluentValidation;
using Spike.AuthenticationServer.IdentityServer.Types.DTOs;

namespace Spike.AuthenticationServer.IdentityServer.Types.Validators
{
    public class ConfirmationValidator: AbstractValidator<AccountConfirmationDto>
    {
        public ConfirmationValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required");
        }
    }
}
