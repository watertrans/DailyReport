using WaterTrans.DailyReport.Application.Abstractions;

namespace WaterTrans.DailyReport.Application.Settings
{
    /// <summary>
    /// アプリケーション設定
    /// </summary>
    public class AppSettings : IAppSettings
    {
        /// <inheritdoc/>
        public int AccessTokenExpiresIn { get; set; }
    }
}
