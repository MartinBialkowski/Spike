using FluentValidation;
using Spike.WebApi.Types.DTOs;

namespace Spike.WebApi.Types.Validators
{
    public class ConfirmationValidator: AbstractValidator<AccountConfirmationDTO>
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
