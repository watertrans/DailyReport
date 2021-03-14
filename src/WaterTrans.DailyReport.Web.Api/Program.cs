using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WaterTrans.DailyReport.Persistence;

namespace WaterTrans.DailyReport.Web.Api
{
    /// <summary>
    /// ���C���֐��N���X
    /// </summary>
    public class Program
    {
        /// <summary>
        /// ���C���֐��B
        /// </summary>
        /// <param name="args">�N���p�����[�^�B</param>
        public static void Main(string[] args)
        {
            DataConfiguration.Initialize();
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// HostBuilder�쐬�B
        /// </summary>
        /// <param name="args">�N���p�����[�^�B</param>
        /// <returns><see cref="IHostBuilder"/></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
