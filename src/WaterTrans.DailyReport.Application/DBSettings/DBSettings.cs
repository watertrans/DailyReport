using System.Data.Common;
using WaterTrans.DailyReport.Application.Abstractions;

namespace WaterTrans.DailyReport.Application.Settings
{
    /// <summary>
    /// データベース接続情報
    /// </summary>
    public class DBSettings : IDBSettings
    {
        /// <inheritdoc/>
        public string StorageConnectionString { get; set; }

        /// <inheritdoc/>
        public string SqlConnectionString { get; set; }

        /// <inheritdoc/>
        public string ReplicaSqlConnectionString { get; set; }

        /// <inheritdoc/>
        public DbProviderFactory SqlProviderFactory { get; set; }

        /// <inheritdoc/>
        public int CommandTimeout { get; set; }
    }
}
