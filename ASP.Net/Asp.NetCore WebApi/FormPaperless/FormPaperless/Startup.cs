using FormPaperless.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using AutoMapper.Configuration;

namespace FormPaperless
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "_DefaultProlicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                // 配置 Cookie 认证选项
                options.Cookie.Name = "MyApp.Cookie";
                //options.LoginPath = "/Account/Login";
                //options.LogoutPath = "/Account/Logout";
                //options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
            });

            //注册服务器缓存
            services.AddMemoryCache();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FormPaperless", Version = "v1" });
                // 如果存在XML文件
                // 则配置 xml 文件路径
                if (File.Exists($@"{System.AppDomain.CurrentDomain.BaseDirectory}\FormPaperless.xml"))
                {
                    c.IncludeXmlComments($@"{System.AppDomain.CurrentDomain.BaseDirectory}\FormPaperless.xml");
                }
            });

            var sCorsUrl = Configuration["AllowedHosts"];
            //添加CORS中间件
            services.AddCors(options => options.AddPolicy(_defaultCorsPolicyName,
            policy =>
            {
                policy.SetIsOriginAllowed(_ => true)
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .AllowCredentials();
            }));

            //添加Session中间件
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);            //Session过期时间
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //添加AutoMapper
            //services.AddAutoMapper(typeof(GenericFormFrofile));

            //注入需要的实例
            services.AddScoped<ServerInfo>();      //服务器信息
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();

            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FormPaperless v1"));
            //}

            //注册CORS跨域资源共享
            app.UseCors(_defaultCorsPolicyName);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            

            app.UseMiddleware<ServerInfoMiddleware>();          //注册服务器信息

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
