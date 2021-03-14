using WaterTrans.DailyReport.Application.Abstractions;

namespace WaterTrans.DailyReport.Persistence.Repositories
{
    /// <summary>
    /// リポジトリ
    /// </summary>
    public abstract class Repository
    {
        /// <summary>
        /// IDBSettings インスタンス
        /// </summary>
        protected IDBSettings DBSettings { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public Repository(IDBSettings dbSettings)
        {
            DBSettings = dbSettings;
        }
    }
}
