using FluentValidation;
using Spike.AuthenticationServer.IdentityServer.Types.DTOs;

namespace Spike.AuthenticationServer.IdentityServer.Types.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Valid email address is required");
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Equal(x => x.ConfirmedPassword).WithMessage("Password and Confirmed Password are differ");
        }
    }
}
