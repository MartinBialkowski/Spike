﻿using Autofac;
using Spike.Backend.Interface.Contact;
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
