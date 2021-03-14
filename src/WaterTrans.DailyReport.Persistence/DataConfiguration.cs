using Dapper.FastCrud;

namespace WaterTrans.DailyReport.Persistence
{
    /// <summary>
    /// データ構成クラス
    /// </summary>
    public static class DataConfiguration
    {
        /// <summary>
        /// 静的コンストラクタ。
        /// </summary>
        public static void Initialize()
        {
            OrmConfiguration.DefaultDialect = SqlDialect.MsSql;
        }
    }
}
