using System;
using Authentication.App.Challenge.Repositories.Database.Redis;
using Autofac;
using StackExchange.Redis;

namespace Authentication.App.Challenge.Repositories
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RedisRepository>()
                .As<IRedisRepository>()
                .SingleInstance();
            
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}