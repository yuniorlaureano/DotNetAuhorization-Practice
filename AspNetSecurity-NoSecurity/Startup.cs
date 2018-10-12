using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetSecurity_NoSecurity.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetSecurity_NoSecurity
{
    public class Startup
    {
        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            if (!_env.IsDevelopment())
            {
                services.Configure<MvcOptions>(options => 
                    options.Filters.Add(new RequireHttpsAttribute()));
            }

            services.AddSingleton<ConferenceRepo>();
            services.AddSingleton<ProposalRepo>();
            services.AddSingleton<AttendeeRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            //The site need to be register in this website for the new browser update to remember
            //hstspreload.appspot.com
            //First we need to install nwebsec for using this middleware
            app.UseHsts(h => h.MaxAge(days: 365).Preload());//this will enforce that the first request is made through https


            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Conference}/{action=Index}/{id?}");
            });
        }
    }
}
