namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// SQLデータベースクエリーサービスインターフェース
    /// </summary>
    public interface ISqlQueryService
    {
        /// <summary>
        /// レプリカデータベースに接続文字列を変更します。
        /// </summary>
        void SwitchReplica();

        /// <summary>
        /// オリジナルデータベースに接続文字列を変更します。
        /// </summary>
        void SwitchOriginal();
    }
}
