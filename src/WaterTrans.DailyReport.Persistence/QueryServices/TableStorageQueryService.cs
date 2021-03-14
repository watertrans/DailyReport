using Microsoft.Azure.Cosmos.Table;
using WaterTrans.DailyReport.Application.Abstractions;

namespace WaterTrans.DailyReport.Persistence.QueryServices
{
    /// <summary>
    /// テーブルストレージクエリーサービス
    /// </summary>
    public abstract class TableStorageQueryService : QueryService, ITableStorageQueryService
    {
        /// <summary>
        /// CloudStorageAccount インスタンス。
        /// </summary>
        protected CloudStorageAccount StorageAccount { get; }

        /// <summary>
        /// CloudTableClient インスタンス。
        /// </summary>
        protected CloudTableClient TableClient { get; }

        /// <summary>
        /// CloudTable インスタンス。
        /// </summary>
        protected CloudTable Table { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public TableStorageQueryService(IDBSettings dbSettings)
            : base(dbSettings)
        {
            StorageAccount = CloudStorageAccount.Parse((string)DBSettings.StorageConnectionString);
            TableClient = StorageAccount.CreateCloudTableClient();
        }
    }
}
