using WaterTrans.DailyReport.Application.Abstractions;

namespace WaterTrans.DailyReport.Application.Settings
{
    /// <summary>
    /// 環境設定
    /// </summary>
    public class EnvSettings : IEnvSettings
    {
        /// <inheritdoc/>
        public bool IsDebug { get; set; }
    }
}
