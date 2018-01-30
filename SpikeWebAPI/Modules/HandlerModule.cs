using Autofac;
using Microsoft.AspNetCore.Authorization;
using Spike.WebApi.Handlers;

namespace Spike.WebApi.Modules
{
    public class HandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StudentDiscountHandler>().As<IAuthorizationHandler>();
        }
    }
}
