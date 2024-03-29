using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NopApp.DAL.Repositories;
using NopApp.Data;
using NopApp.Models.DbModels;
using NopApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NopApp.Core
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NopApp.Core", Version = "v1" });
            });

            services.AddDbContext<NopAppContext>(options => options.UseSqlServer(Configuration.GetConnectionString("NopAppContext")));
            services.AddIdentity<User, Role>(options => 
                { 
                    options.User.RequireUniqueEmail = true; 
                })
                .AddEntityFrameworkStores<NopAppContext>();

            services.AddScoped<UserRepository>();
            services.AddScoped<KitchenRepository>();
            services.AddScoped<AuthenticationService>();
            services.AddScoped<KitchenService>();
            
            //var serviceProvider = services.BuildServiceProvider();
            //var service = serviceProvider.GetService<KitchenService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NopApp.Core v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
