using Dapper;
using System.Data;
using System.Data.Common;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Persistence.Resources;

namespace WaterTrans.DailyReport.Persistence
{
    /// <summary>
    /// データベースセットアップ
    /// </summary>
    public class DataSetup
    {
        /// <summary>
        /// IDBSettings インスタンス
        /// </summary>
        protected IDBSettings DBSettings { get; }

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
        public DataSetup(IDBSettings dbSettings)
        {
            DBSettings = dbSettings;
            Factory = DBSettings.SqlProviderFactory;
            Connection = Factory.CreateConnection();
            Connection.ConnectionString = DBSettings.SqlConnectionString;
        }

        /// <summary>
        /// データベースを構築します。
        /// </summary>
        public void Initialize()
        {
            CreateTables();
        }

        /// <summary>
        /// データベースをクリーンアップします。
        /// </summary>
        public void Cleanup()
        {
            DropTables();
        }

        /// <summary>
        /// データベースのテーブルを構築します。
        /// </summary>
        public void CreateTables()
        {
            Connection.Execute(SqlSchema.CreateAccessToken);
            Connection.Execute(SqlSchema.CreateApplication);
            Connection.Execute(SqlSchema.CreateGroup);
            Connection.Execute(SqlSchema.CreateGroupPerson);
            Connection.Execute(SqlSchema.CreatePerson);
            Connection.Execute(SqlSchema.CreateProject);
            Connection.Execute(SqlSchema.CreateTag);
            Connection.Execute(SqlSchema.CreateWorkType);
        }

        /// <summary>
        /// 初期データを取り込みます。
        /// </summary>
        public void LoadInitialData()
        {
            Connection.Execute(SqlSchema.LoadInitialData);
        }

        /// <summary>
        /// ユニットテスト用のデータを取り込みます。
        /// </summary>
        public void LoadUnitTestData()
        {
            Connection.Execute(SqlSchema.LoadUnitTestData);
        }

        /// <summary>
        /// データベースのテーブルを削除します。
        /// </summary>
        public void DropTables()
        {
            Connection.Execute("DROP TABLE IF EXISTS AccessToken");
            Connection.Execute("DROP TABLE IF EXISTS Application");
            Connection.Execute("DROP TABLE IF EXISTS [Group]");
            Connection.Execute("DROP TABLE IF EXISTS GroupPerson");
            Connection.Execute("DROP TABLE IF EXISTS Person");
            Connection.Execute("DROP TABLE IF EXISTS Project");
            Connection.Execute("DROP TABLE IF EXISTS Tag");
            Connection.Execute("DROP TABLE IF EXISTS WorkType");
        }
    }
}
