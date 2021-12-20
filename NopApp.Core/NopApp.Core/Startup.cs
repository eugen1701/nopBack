using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NopApp.DAL.Repositories;
using NopApp.Data;
using NopApp.Models.DbModels;
using NopApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopApp.WebApi.Options;

namespace NopApp.Core
{
    public class Startup
    {
        readonly string NopAppAllowSpecificOrigins = "_nopAppAllowSpecificOrigins";

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

            var appSettingsSection = Configuration.GetSection("JwtOptions");
            services.Configure<JwtOptions>(appSettingsSection);

            services.AddCors(options =>
            {
                options.AddPolicy(name: NopAppAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:8100", "http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
                                  });
            });

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<JwtOptions>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddDbContext<NopAppContext>(options => options.UseSqlServer(Configuration.GetConnectionString("NopAppContext")));
            services.AddIdentity<User, Role>(options => 
                { 
                    options.User.RequireUniqueEmail = true; 
                })
                .AddEntityFrameworkStores<NopAppContext>();

            services.AddScoped<UserRepository>();
            services.AddScoped<KitchenRepository>();
            services.AddScoped<OfferRepository>();

            services.AddScoped<IngredientRepository>();
            services.AddScoped<MealRepository>();
            services.AddScoped<AuthenticationService>();
            services.AddScoped<KitchenService>();
            services.AddScoped<UserService>();
            services.AddScoped<OfferService>();
            
            services.AddScoped<IngredientService>();
            services.AddScoped<MealService>();
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

            app.UseCors(NopAppAllowSpecificOrigins);

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
