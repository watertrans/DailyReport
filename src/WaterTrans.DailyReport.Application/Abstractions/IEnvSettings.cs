namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 環境設定インターフェース
    /// </summary>
    public interface IEnvSettings
    {
        /// <summary>
        /// デバッグモードかどうかを取得します。
        /// </summary>
        bool IsDebug { get; }
    }
}
