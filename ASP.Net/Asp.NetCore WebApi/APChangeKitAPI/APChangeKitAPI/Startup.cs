using APChangeKitAPI.Common;
using APChangeKitAPI.Filters;
using APChangeKitAPI.Middleware;
using APChangeKitAPI.Services.AutoMapper;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace APChangeKitAPI
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "MyPolicy";
        public IConfiguration _configuration { get; }
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //��ʼ������
            AppSettings.Init(_configuration);

            // ���Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "APChangeKit API", Version = "v1" });
                // ��ȡxml�ļ���
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // ��ȡxml�ļ�·��
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // ��ӿ�������ע�ͣ�true��ʾ��ʾ������ע��
                c.IncludeXmlComments(xmlPath, true);
            });

            services.AddAutoMapper(typeof(OrganizationProfile));

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                builder =>
                {
                    builder
                    .SetIsOriginAllowed(_ => true)
                    //.WithOrigins(AppSettings.Data.AllowedHostsCors.Split(','))
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    //����ǰ�˻�ȡ����ͷ
                    .WithExposedHeaders("Content-Disposition", "Authorization");
                });
            });

            services.AddMvc(option =>
            {
                //ע��ȫ���쳣����
                option.Filters.Add(typeof(ExceptionFilter));
            }).AddNewtonsoftJson(options =>
            {
                //����ѭ������
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // ���Swagger�й��м��
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "APChangeKit API v1");
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            //app.UseAuthorization();
            app.UseRequestResponseLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// AutoFac����ע��
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //ע��Application��
            var asm = Assembly.Load("APChangeKitAPI");//ָ��dll���Ƶĳ��򼯼�
            builder.RegisterAssemblyTypes(asm)
                .PublicOnly();//��ע��public�ķ���
                //.Where(x => x.IsClass);//ֻҪclass�ͣ���ҪΪ���ų�ֵ��interface���ͣ�
                              //.AsImplementedInterfaces();
                              //.Except()�ų�ĳ����
                              //.Where(t => t.Name.EndsWith("Service") || t.Name == "ClassA")//����������дһЩ������������
                              //AutofacExt.InitAutofac(builder);
        }
    }
}
