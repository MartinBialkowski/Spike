using Autofac;
using FluentValidation;
using Spike.WebApi.Types.DTOs;
using Spike.WebApi.Types.Validators;

namespace Spike.WebApi.Modules
{
    public class ValidatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserValidator>().As<IValidator<UserDTO>>();
            builder.RegisterType<ConfirmationValidator>().As<IValidator<AccountConfirmationDTO>>();
            builder.RegisterType<ResetPasswordValidator>().As<IValidator<ResetPasswordDTO>>();
            builder.RegisterType<ClaimAssignmentValidator>().As<IValidator<ClaimDTO>>();
            builder.RegisterType<ValidatorFactory>().As<IValidatorFactory>().SingleInstance();
        }
    }
}
