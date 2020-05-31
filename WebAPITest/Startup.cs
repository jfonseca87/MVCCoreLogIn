using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebAPITest.Models;
using WebAPITest.Services;
using WebAPITest.Utils;

namespace WebAPITest
{
    public class Startup
    {
        private readonly IConfiguration _conf;

        public Startup(IConfiguration conf)
        {
            _conf = conf;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            Global.ConnectionString = _conf["ConnectionsString:Connection"];

            services.AddDbContext<SecurityApplicationContext>(options => {
                options.UseSqlServer(_conf["ConnectionsString:Connection"]);
            });

            services.AddTransient<AunthenticateMethod1>();
            services.AddTransient<AuthenticateMethod2>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
