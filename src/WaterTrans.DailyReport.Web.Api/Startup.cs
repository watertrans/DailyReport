using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.OpenApi.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.Services;
using WaterTrans.DailyReport.Application.Settings;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Persistence.QueryServices;
using WaterTrans.DailyReport.Persistence.Repositories;
using WaterTrans.DailyReport.Web.Api.AttributeAdapters;
using WaterTrans.DailyReport.Web.Api.Filters;
using WaterTrans.DailyReport.Web.Api.ObjectResults;
using WaterTrans.DailyReport.Web.Api.Resources;
using WaterTrans.DailyReport.Web.Api.Security;

namespace WaterTrans.DailyReport.Web.Api
{
    /// <summary>
    /// スタートアップクラス
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <param name="environment"><see cref="IWebHostEnvironment"/></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            WebHostEnvironment = environment;
        }

        /// <summary>
        /// IConfiguration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// IWebHostEnvironment
        /// </summary>
        public IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// サービス構成処理。
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<EnvSettings>(Configuration.GetSection("EnvSettings"));
            services.Configure<DBSettings>(options =>
            {
                Configuration.GetSection("DBSettings").Bind(options);
                options.SqlProviderFactory = SqlClientFactory.Instance;
            });

            // This is work around. See https://github.com/dotnet/aspnetcore/issues/4853 @zhurinvlad commented on 5 Sep 2018
            services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();
            services.AddSingleton<IConfigureOptions<MvcOptions>, MvcConfiguration>();

            services.AddTransient<IAppSettings>(x => x.GetService<IOptionsMonitor<AppSettings>>().CurrentValue);
            services.AddTransient<IEnvSettings>(x => x.GetService<IOptionsMonitor<EnvSettings>>().CurrentValue);
            services.AddTransient<IDBSettings>(x => x.GetService<IOptionsMonitor<DBSettings>>().CurrentValue);
            services.AddTransient<IAccessTokenRepository, AccessTokenRepository>();
            services.AddTransient<IAccountQueryService, AccountQueryService>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IApplicationRepository, ApplicationRepository>();
            services.AddTransient<IAuthorizationCodeRepository, AuthorizationCodeRepository>();
            services.AddTransient<IAuthorizeService, AuthorizeService>();
            services.AddTransient<IGroupPersonRepository, GroupPersonRepository>();
            services.AddTransient<IGroupQueryService, GroupQueryService>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IPersonQueryService, PersonQueryService>();
            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IProjectPersonRepository, ProjectPersonRepository>();
            services.AddTransient<IProjectQueryService, ProjectQueryService>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<IWorkTypeRepository, WorkTypeRepository>();

            if (!WebHostEnvironment.IsProduction())
            {
                services.AddCors(options =>
                {
                    options.AddDefaultPolicy(builder =>
                    {
                        builder.WithOrigins("https://editor.swagger.io")
                            .AllowCredentials()
                            .AllowAnyMethod()
                            .AllowAnyHeader();

                        builder.WithOrigins("https://watertrans.stoplight.io")
                            .AllowCredentials()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
                });
            }

            services.AddAuthentication(BearerAuthenticationHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, BearerAuthenticationHandler>(BearerAuthenticationHandler.SchemeName, null);

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(options =>
                {
                    Configuration.Bind("AzureAD", options);
                    options.Events.OnSignedOutCallbackRedirect = context =>
                    {
                        context.Response.Redirect("/");
                        context.HandleResponse();
                        return Task.CompletedTask;
                    };
                    options.Events.OnTokenValidated = context =>
                    {
                        return Task.CompletedTask;
                    };
                });

            services.AddRazorPages().AddMicrosoftIdentityUI();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.FullControlScopePolicy, policy =>
                {
                    policy.RequireClaim(Scopes.ClaimType, Scopes.FullControl);
                });
                options.AddPolicy(Policies.ReadScopePolicy, policy =>
                {
                    policy.RequireClaim(Scopes.ClaimType, Scopes.FullControl, Scopes.Read, Scopes.Write);
                });
                options.AddPolicy(Policies.WriteScopePolicy, policy =>
                {
                    policy.RequireClaim(Scopes.ClaimType, Scopes.FullControl, Scopes.Write);
                });
            });

            services.AddApiVersioning();
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(CatchAllExceptionFilter));
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.IgnoreNullValues = true;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    return ErrorObjectResultFactory.ValidationError(context.ModelState);
                };
            })
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(ErrorMessages));
            });
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.SwaggerGeneratorOptions.DescribeAllParametersInCamelCase = true;
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "DailyReport Web Api", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "アクセストークンを入力してください。",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        new string[] { }
                    },
                });
            });
        }

        /// <summary>
        /// アプリケーション構成処理。
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/></param>
        /// <param name="env"><see cref="IWebHostEnvironment"/></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStatusCodePages(async context =>
            {
                var options = context.HttpContext.RequestServices.GetService<IOptions<JsonOptions>>();
                string json;

                if (context.HttpContext.Response.StatusCode == 404)
                {
                    json = JsonSerializer.Serialize(ErrorObjectResultFactory.NotFound().Value, options.Value.JsonSerializerOptions);
                }
                else
                {
                    return;
                }

                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(json);
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = string.Empty;
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "WaterTrans.Scheduler.WebApi v1");
            });
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
        }
    }
}
