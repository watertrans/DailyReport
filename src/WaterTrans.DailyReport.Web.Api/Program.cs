using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WaterTrans.DailyReport.Persistence;

namespace WaterTrans.DailyReport.Web.Api
{
    /// <summary>
    /// メイン関数クラス
    /// </summary>
    public class Program
    {
        /// <summary>
        /// メイン関数。
        /// </summary>
        /// <param name="args">起動パラメータ。</param>
        public static void Main(string[] args)
        {
            DataConfiguration.Initialize();
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// HostBuilder作成。
        /// </summary>
        /// <param name="args">起動パラメータ。</param>
        /// <returns><see cref="IHostBuilder"/></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
