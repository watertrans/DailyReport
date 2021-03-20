using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using WaterTrans.DailyReport.Application.Settings;
using WaterTrans.DailyReport.Persistence;

namespace WaterTrans.DailyReport.UnitTests
{
    [TestClass]
    public class TestEnvironment
    {
        public static string WebApiBaseAddress { get; private set; }
        public static DBSettings DBSettings { get; } = new DBSettings();
        private static Process _process;

        [AssemblyInitialize]
        public static void Initialize(TestContext _)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("testsettings.json");

            var configuration = builder.Build();
            configuration.GetSection("DBSettings").Bind(DBSettings);
            WebApiBaseAddress = configuration["WebApiBaseAddress"];
            DBSettings.SqlProviderFactory = SqlClientFactory.Instance;

            DataConfiguration.Initialize();
            var setup = new DataSetup(DBSettings);
            setup.Initialize();
            setup.LoadUnitTestData();

            string webApiProjectName = "WaterTrans.DailyReport.Web.Api";
            string testProjectName = Assembly.GetExecutingAssembly().GetName().Name;
            string solutionRootDirectory = Environment.CurrentDirectory.Split(testProjectName)[0];
            string webApiProjectDirectory = Path.Combine(solutionRootDirectory, webApiProjectName);

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --launch-profile \"{webApiProjectName}\"",
                WorkingDirectory = webApiProjectDirectory,
            };

            startInfo.EnvironmentVariables["DBSettings__StorageConnectionString"] = DBSettings.StorageConnectionString;
            startInfo.EnvironmentVariables["DBSettings__SqlConnectionString"] = DBSettings.SqlConnectionString;
            startInfo.EnvironmentVariables["DBSettings__ReplicaSqlConnectionString"] = DBSettings.ReplicaSqlConnectionString;

            _process = Process.Start(startInfo);

            StartupWebApiProject();
        }

        private static void StartupWebApiProject()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = delegate { return true; };
            var httpclient = new HttpClient(httpClientHandler);
            httpclient.BaseAddress = new Uri(TestEnvironment.WebApiBaseAddress);

            HttpResponseMessage response;
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    response = httpclient.GetAsync("swagger").ConfigureAwait(false).GetAwaiter().GetResult();
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
            if (_process != null && !_process.HasExited)
            {
                _process.Kill();
            }
        }
    }
}
