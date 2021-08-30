using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SunriseSunsetDataProvider.Interface;
using SunriseSunsetDataProvider.Model;
using SunriseSunsetDataProvider.Module;
using SunriseSunsetDataProvider.VariableClasses;

namespace SunriseSunsetDataProvider
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            var connectionData = Configuration.GetSection("SunriseAndSunsetDataDB").Get<SqlConnectionInfo>();

            IRepositoryOperater repository = new Repository(connectionData.GetSqlConnectionStringBuilder());
            services.AddSingleton(repository);

            IOpenDataOperater openData = new OpenDataService(Configuration.GetValue<string>("OpenDataURL"), new HttpClient());
            services.AddSingleton(openData);

            services.AddSingleton<DataService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    // name: 攸關 SwaggerDocument 的 URL 位置。
                    name: "v1",
                    // info: 是用於 SwaggerDocument 版本資訊的顯示(內容非必填)。
                    info: new OpenApiInfo
                    {
                        Title = "SunriseSunsetDataProvider API",
                        Version = "v1",
                        Description = "全臺各縣市日出日沒時刻資料API"
                    }
                );
                c.IncludeXmlComments($"{AppContext.BaseDirectory}SunriseSunsetDataProvider.xml");
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
           
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SunriseSunsetDataProvider API");
            });
        }
    }
}
