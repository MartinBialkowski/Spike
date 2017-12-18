using FluentValidation;
using SpikeWebAPI.DTOs;

namespace SpikeWebAPI.Validators
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
