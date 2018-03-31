using Autofac;
using FluentValidation;
using Spike.AuthenticationServer.IdentityServer.Types.DTOs;
using Spike.AuthenticationServer.IdentityServer.Types.Validators;

namespace Spike.AuthenticationServer.IdentityServer.Modules
{
    public class ValidatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserValidator>().As<IValidator<UserDto>>();
            builder.RegisterType<ConfirmationValidator>().As<IValidator<AccountConfirmationDto>>();
            builder.RegisterType<ResetPasswordValidator>().As<IValidator<ResetPasswordDto>>();
            builder.RegisterType<ClaimValidator>().As<IValidator<ClaimDto>>();
            builder.RegisterType<ValidatorFactory>().As<IValidatorFactory>().SingleInstance();
        }
    }
}
