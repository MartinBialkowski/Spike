using Autofac;
using Spike.Backend.Interface.Contact;
using Spike.Backend.Connect;
using Spike.Backend.Connect.Services;

namespace Spike.AuthenticationServer.IdentityServer.Modules
{
    public class SenderProviderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SendGridEmailSender>().As<IEmailSender>();
        }
    }
}
