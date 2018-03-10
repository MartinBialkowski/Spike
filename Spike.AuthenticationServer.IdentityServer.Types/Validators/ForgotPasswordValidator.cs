using FluentValidation;
using Spike.AuthenticationServer.IdentityServer.Types.DTOs;

namespace Spike.AuthenticationServer.IdentityServer.Types.Validators
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDTO>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required");
        }
    }
}
