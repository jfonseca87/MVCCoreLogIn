using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using WebAPITest2.Models;
using WebAPITest2.Services;
using WebAPITest2.Utils;

namespace WebAPITest2
{
    public class Startup
    {
        private readonly IConfiguration conf;

        public Startup(IConfiguration _conf)
        {
            conf = _conf;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            Global.ConnectionString = conf["ConnectionStrings:Default"];
            services.AddDbContext<SecurityApplicationContext>(options => options.UseSqlServer(conf["ConnectionStrings:Default"]));

            string strKey = conf.GetValue<string>("TokenKey");
            Global.Key = strKey;
            byte[] key = Encoding.ASCII.GetBytes(strKey);

            services.AddAuthentication("Basic").AddScheme<CustomAuthenticationOptions, CustomAuthenticationHandler>("Basic", null);

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(config =>
            //{
            //    config.RequireHttpsMetadata = false;
            //    config.SaveToken = true;
            //    config.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});

            services.AddTransient<Method1>();
            services.AddTransient<Method2>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
