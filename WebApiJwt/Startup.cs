using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebApiJwt.Models;

namespace WebApiJwt
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => 
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = "yuni@gmail.com",
                            ValidAudience = "yuni@gmail.com",
                            IssuerSigningKey = new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(_configuration["SecurityKey"])
                                )
                        };
                    });
            services.AddSingleton<IAuthorizationHandler, MinimumMonthsEmployedHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("TrainedStaffOnly",
                        policy => policy.RequireClaim(CustomClaims.CompletedBasicTraining)
                                        .AddRequirements(new MinimumMonthsEmployedRequirement(3)));
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseMvc();
            app.UseStaticFiles();
        }
    }
}
