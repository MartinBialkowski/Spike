using FluentValidation;
using Spike.WebApi.Types.DTOs;

namespace Spike.WebApi.Types.Validators
{
    public class ClaimAssignmentValidator : AbstractValidator<ClaimDTO>
    {
        public ClaimAssignmentValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("ClaimType is required");

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("ClaimValue is required");
        }
    }
}
