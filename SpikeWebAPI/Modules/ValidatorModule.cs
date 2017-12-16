using Autofac;
using FluentValidation;
using SpikeWebAPI.DTOs;
using SpikeWebAPI.Validators;

namespace SpikeWebAPI.Modules
{
    public class ValidatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoginValidator>().As<IValidator<UserDTO>>();
            builder.RegisterType<ConfirmationValidator>().As<IValidator<AccountConfirmationDTO>>();
            builder.RegisterType<ValidatorFactory>().As<IValidatorFactory>().SingleInstance();
        }
    }
}
