using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using WaterTrans.DailyReport.Application.Settings;
using WaterTrans.DailyReport.Persistence;
using WaterTrans.DailyReport.Web.Api;

namespace WaterTrans.DailyReport.Tests
{
    [TestClass]
    public class TestEnvironment
    {
        public static WebApplicationFactory<Startup> WebApiFactory;
        public static DBSettings DBSettings { get; } = new DBSettings();

        [AssemblyInitialize]
        public static void Initialize(TestContext _)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("testsettings.json");

            var configuration = builder.Build();
            configuration.GetSection("DBSettings").Bind(DBSettings);
            DBSettings.SqlProviderFactory = SqlClientFactory.Instance;

            DataConfiguration.Initialize();
            var setup = new DataSetup(DBSettings);
            setup.Initialize();
            setup.LoadUnitTestData();

            WebApiFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.Configure<DBSettings>(options =>
                        {
                            options.StorageConnectionString = DBSettings.StorageConnectionString;
                            options.SqlConnectionString = DBSettings.SqlConnectionString;
                            options.ReplicaSqlConnectionString = DBSettings.ReplicaSqlConnectionString;
                        });
                    });
                });

            StartupWebApiProject();
        }

        private static void StartupWebApiProject()
        {
            var httpclient = WebApiFactory.CreateClient();

            HttpResponseMessage response;
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    response = httpclient.GetAsync("/").ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (Exception)
                {
                    response = null;
                }
                
                if (response != null && response.IsSuccessStatusCode)
                {
                    break;
                }
                Thread.Sleep(200);
            }
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            var setup = new DataSetup(DBSettings);
            setup.Cleanup();
        }
    }
}
