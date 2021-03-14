using WaterTrans.DailyReport.Application.Abstractions;

namespace WaterTrans.DailyReport.Persistence.QueryServices
{
    /// <summary>
    /// クエリーサービス
    /// </summary>
    public abstract class QueryService
    {
        /// <summary>
        /// IDBSettings インスタンス
        /// </summary>
        protected IDBSettings DBSettings { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public QueryService(IDBSettings dbSettings)
        {
            DBSettings = dbSettings;
        }
    }
}
