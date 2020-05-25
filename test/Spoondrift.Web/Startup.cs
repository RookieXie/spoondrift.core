using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spoondrift.Code.Dapper;
using Spoondrift.Code.PlugIn;
using Spoondrift.Code.Util;

namespace Spoondrift.Web
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
            //²å¼þ¼ÓÔØ
            services.AddCodePlugService();
            services.AddScoped<IUnitOfDapper>(p =>
            {
                return new DapperContext("server=118.24.146.107;port=3306;uid=root;pwd=mypassword;database=Spoondrift;Min Pool Size=0;Pooling=true;connect timeout=120;CharSet=utf8mb4;SslMode=none;");
            });
            services.AddScoped<Xml2DB>();
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
