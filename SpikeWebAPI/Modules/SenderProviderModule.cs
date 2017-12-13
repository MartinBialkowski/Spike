using Autofac;
using SpikeConnectProviders;
using SpikeConnectProviders.Abstract;

namespace SpikeWebAPI.Modules
{
    public class SenderProviderModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SendGridEmailSender>().As<IEmailSender>();
        }
    }
}
