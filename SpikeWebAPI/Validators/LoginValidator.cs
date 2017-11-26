using FluentValidation;
using SpikeWebAPI.DTOs;

namespace SpikeWebAPI.Validators
{
    public class LoginValidator : AbstractValidator<UserDTO>
    {
        public LoginValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(login => login.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("A valid email address is required.");
            

        }
    }
}
