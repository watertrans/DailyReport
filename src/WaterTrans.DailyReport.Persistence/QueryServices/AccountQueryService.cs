using Dapper;
using System;
using System.Text;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.Utils;

namespace WaterTrans.DailyReport.Persistence.QueryServices
{
    /// <summary>
    /// アカウントクエリーサービス
    /// </summary>
    public class AccountQueryService : SqlQueryService, IAccountQueryService
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public AccountQueryService(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }

        /// <inheritdoc/>
        public int Count()
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT COUNT(*) ");
            sql.AppendLine("   FROM Account ");

            var result = Connection.ExecuteScalar(sql.ToString(), null, commandTimeout: DBSettings.CommandTimeout);
            return int.Parse(result.ToString());
        }

        /// <inheritdoc/>
        public void UpdateLastLoginTime(Guid accountId)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" UPDATE Account ");
            sql.AppendLine("    SET LastLoginTime = @LastLoginTime ");
            sql.AppendLine("  WHERE AccountId = @AccountId ");

            var param = new
            {
                AccountId = accountId,
                LastLoginTime = DateUtil.Now,
            };

            Connection.Execute(sql.ToString(), param, commandTimeout: DBSettings.CommandTimeout);
        }
    }
}
