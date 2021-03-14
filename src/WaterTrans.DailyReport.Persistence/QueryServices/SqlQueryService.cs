using System;
using System.Data;
using System.Data.Common;
using WaterTrans.DailyReport.Application.Abstractions;

namespace WaterTrans.DailyReport.Persistence.QueryServices
{
    /// <summary>
    /// SQLデータベースクエリーサービス
    /// </summary>
    /// <remarks>
    /// SQLデータベースクエリーサービスは、SwitchReplicaメソッドの呼び出しによってレプリカデータベースにアクセスできます。
    /// 更新系の処理と衝突しませんので、同時実行性が高まりますがレプリカにデータが反映されるまで古いデータを参照します。
    /// </remarks>
    public abstract class SqlQueryService : QueryService, ISqlQueryService
    {
        private readonly string _sqlConnectionString;
        private readonly string _replicaSqlConnectionString;

        /// <summary>
        /// IDbConnection
        /// </summary>
        protected IDbConnection Connection { get; }

        /// <summary>
        /// DbProviderFactory
        /// </summary>
        protected DbProviderFactory Factory { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public SqlQueryService(IDBSettings dbSettings)
            : base(dbSettings)
        {
            Factory = DBSettings.SqlProviderFactory;
            Connection = Factory.CreateConnection();
            Connection.ConnectionString = DBSettings.SqlConnectionString;
            _sqlConnectionString = DBSettings.SqlConnectionString;
            _replicaSqlConnectionString = DBSettings.ReplicaSqlConnectionString;
        }

        /// <inheritdoc/>
        public void SwitchReplica()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                throw new InvalidOperationException("The connection was not closed.");
            }

            Connection.ConnectionString = _replicaSqlConnectionString;
        }

        /// <inheritdoc/>
        public void SwitchOriginal()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                throw new InvalidOperationException("The connection was not closed.");
            }

            Connection.ConnectionString = _sqlConnectionString;
        }
    }
}
