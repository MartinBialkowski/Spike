using FluentValidation;
using Spike.AuthenticationServer.IdentityServer.Types.DTOs;

namespace Spike.AuthenticationServer.IdentityServer.Types.Validators
{
    public class UserValidator : AbstractValidator<UserDTO>
    {
        public UserValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(login => login.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("A valid email address is required.");
            RuleFor(login => login.Password).NotEmpty().WithMessage("Password is required");

        }
    }
}
