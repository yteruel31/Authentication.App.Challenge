using System;
using Autofac;

namespace Authentication.App.Challenge.Repositories
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}