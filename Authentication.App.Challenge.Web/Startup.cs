using Authentication.App.Challenge.Repositories.Database;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ServicesModule = Authentication.App.Challenge.Services.AutofacModule;
using RepositoriesModule = Authentication.App.Challenge.Repositories.AutofacModule;

namespace Authentication.App.Challenge.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public ILifetimeScope AutofacContainer { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });
            
            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "AuthApi",
                    Description = "API for authentication",
                    Contact = new OpenApiContact
                    {
                        Name = "Yoann TERUEL",
                        Email = "yoann.teruel@gmail.com"
                    }
                })
            );
        }
        
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<ServicesModule>();
            builder.RegisterModule<RepositoriesModule>();
            builder
                .RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>()
                .SingleInstance();
            builder.RegisterType<CoreContext>()
                .WithParameter(new TypedParameter(typeof(DbContextOptionsBuilder<CoreContext>), 
                    DbContextOptionsFactory.Get()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            CoreContext context = AutofacContainer.Resolve<CoreContext>();
            
            context.Database.Migrate();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthAPI V1");
                c.RoutePrefix = "doc";
            });
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer("start");
                }
            });
        }
    }
}