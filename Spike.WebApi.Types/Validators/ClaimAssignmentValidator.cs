using FluentValidation;
using Spike.WebApi.Types.DTOs;

namespace Spike.WebApi.Types.Validators
{
    public class ClaimAssignmentValidator : AbstractValidator<ClaimAssignmentDTO>
    {
        public ClaimAssignmentValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Valid email address is required");

            RuleFor(x => x.ClaimType)
                .NotEmpty().WithMessage("ClaimType is required");

            RuleFor(x => x.ClaimValue)
                .NotEmpty().WithMessage("ClaimValue is required");
        }
    }
}
