using System.Text.Json.Serialization;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using AutoMapper.EquivalencyExpression;
using AutoMapper.Extensions.EnumMapping;
using ContactApp.Helpers;
using ContactApp.Helpers.Auth;
using ContactApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ContactApp
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
            ConfigureJsonOptions(services);
            
            services.AddDbContext<ApplicationContext>();
            
            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            
            // configure cors
            ConfigureCors(services);
            
            // DI
            ConfigureDependencyInjections(services);

            services.AddHttpContextAccessor();

            ConfigureAutoMapper(services);
            //services.AddAutoMapper(typeof(Startup));
            

            ConfigureSwaggerAuth(services);
            
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("DevelopmentPolicy");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContactApp v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            EnsureDatabaseIsCreated();
            
            // JWT Middleware
            app.UseMiddleware<JwtMiddleware>();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper((automapper) =>
            {
                automapper.EnableEnumMappingValidation();
            }, typeof(Startup));
        }

        private static void EnsureDatabaseIsCreated()
        {
            using var client = new ApplicationContext();
            
            client.Database.EnsureCreated();
        }

        private static void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("DevelopmentPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }));
        }

        private static void ConfigureSwaggerAuth(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ContactApp", Version = "v1"});
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description =
                            "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                    }
                );
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

        private static void ConfigureDependencyInjections(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IContactService, ContactService>();
        }

        private static void ConfigureJsonOptions(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
        }
    }
}