using Autofac;
using Infrastructure.ContactProvider;
using SpikeConnectProviders;

namespace SpikeWebAPI.Modules
{
    public class SenderProviderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SendGridEmailSender>().As<IEmailSender>();
        }
    }
}
