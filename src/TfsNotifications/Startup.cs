using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TfsNotifications.Infrastructure;
using TfsNotifications.Interfaces;
using TfsNotifications.Transmisson;

namespace TfsNotifications
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<IMapper, Mapper>()
                .AddScoped<INotificationTransmitter, NotificationTransmitter>();

            services.Configure<Infrastructure.Options.Notifications>(Configuration.GetSection(nameof(Infrastructure.Options.Notifications)));

            services
                .AddMvc(o => o.Conventions.Add(new RoutePrefixConvention(new RouteAttribute("api"))))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}