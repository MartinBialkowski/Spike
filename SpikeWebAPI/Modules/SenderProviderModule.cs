using Autofac;
using Spike.Service.Interface.ContactProvider;
using Spike.Backend.Connect;

namespace Spike.WebApi.Modules
{
    public class SenderProviderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SendGridEmailSender>().As<IEmailSender>();
        }
    }
}
