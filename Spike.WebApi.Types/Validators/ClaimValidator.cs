using FluentValidation;
using Spike.WebApi.Types.DTOs;

namespace Spike.WebApi.Types.Validators
{
    public class ClaimValidator : AbstractValidator<ClaimDTO>
    {
        public ClaimValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("ClaimType is required");

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("ClaimValue is required");
        }
    }
}
