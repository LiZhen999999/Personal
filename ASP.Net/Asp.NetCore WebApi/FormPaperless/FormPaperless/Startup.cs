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
                // ���� Cookie ��֤ѡ��
                options.Cookie.Name = "MyApp.Cookie";
                //options.LoginPath = "/Account/Login";
                //options.LogoutPath = "/Account/Logout";
                //options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
            });

            //ע�����������
            services.AddMemoryCache();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FormPaperless", Version = "v1" });
                // �������XML�ļ�
                // ������ xml �ļ�·��
                if (File.Exists($@"{System.AppDomain.CurrentDomain.BaseDirectory}\FormPaperless.xml"))
                {
                    c.IncludeXmlComments($@"{System.AppDomain.CurrentDomain.BaseDirectory}\FormPaperless.xml");
                }
            });

            var sCorsUrl = Configuration["AllowedHosts"];
            //���CORS�м��
            services.AddCors(options => options.AddPolicy(_defaultCorsPolicyName,
            policy =>
            {
                policy.SetIsOriginAllowed(_ => true)
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .AllowCredentials();
            }));

            //���Session�м��
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);            //Session����ʱ��
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //���AutoMapper
            //services.AddAutoMapper(typeof(GenericFormFrofile));

            //ע����Ҫ��ʵ��
            services.AddScoped<ServerInfo>();      //��������Ϣ
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

            //ע��CORS������Դ����
            app.UseCors(_defaultCorsPolicyName);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            

            app.UseMiddleware<ServerInfoMiddleware>();          //ע���������Ϣ

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
