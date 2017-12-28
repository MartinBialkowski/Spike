using FluentValidation;
using SpikeWebAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeWebAPI.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDTO>
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
