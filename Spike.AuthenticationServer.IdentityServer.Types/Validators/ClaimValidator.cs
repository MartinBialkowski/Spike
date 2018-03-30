using FluentValidation;
using Spike.AuthenticationServer.IdentityServer.Types.DTOs;

namespace Spike.AuthenticationServer.IdentityServer.Types.Validators
{
    public class ClaimValidator : AbstractValidator<ClaimDto>
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
