using Autofac;
using Microsoft.AspNetCore.Authorization;
using Spike.WebApi.Handlers;

namespace Spike.WebApi.Modules
{
    public class AuthorizationHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StudentDiscountHandler>().As<IAuthorizationHandler>();
            builder.RegisterType<SelfHandler>().As<IAuthorizationHandler>();
        }
    }
}
