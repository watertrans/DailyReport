using System.Data.Common;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// データベース接続情報インターフェース
    /// </summary>
    public interface IDBSettings
    {
        /// <summary>
        /// ストレージアカウントの接続文字列を取得します。
        /// </summary>
        string StorageConnectionString { get; }

        /// <summary>
        /// SQLデータベースの接続文字列を取得します。
        /// </summary>
        string SqlConnectionString { get; }

        /// <summary>
        /// レプリカSQLデータベースの接続文字列を取得します。
        /// </summary>
        string ReplicaSqlConnectionString { get; }

        /// <summary>
        /// SQLデータベースのデータベースプロバイダを取得します。
        /// </summary>
        DbProviderFactory SqlProviderFactory { get; }

        /// <summary>
        /// コマンドタイムアウトを取得します。
        /// </summary>
        int CommandTimeout { get; }
    }
}
