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
            builder.RegisterType<UserValidator>().As<IValidator<UserDTO>>();
            builder.RegisterType<ConfirmationValidator>().As<IValidator<AccountConfirmationDTO>>();
            builder.RegisterType<ResetPasswordValidator>().As<IValidator<ResetPasswordDTO>>();
            builder.RegisterType<ClaimValidator>().As<IValidator<ClaimDTO>>();
            builder.RegisterType<ValidatorFactory>().As<IValidatorFactory>().SingleInstance();
        }
    }
}
