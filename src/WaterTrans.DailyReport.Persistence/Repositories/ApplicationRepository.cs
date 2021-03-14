using Dapper;
using System.Text;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Persistence.Repositories
{
    /// <summary>
    /// アプリケーションリポジトリ
    /// </summary>
    public class ApplicationRepository : SqlRepository<ApplicationTableEntity>, IApplicationRepository
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public ApplicationRepository(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }

        /// <inheritdoc/>
        public ApplicationTableEntity FindByClientId(string clientId)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT * ");
            sql.AppendLine(" FROM   Application ");
            sql.AppendLine(" WHERE  ClientId = @ClientId ");

            var param = new
            {
                ClientId = clientId,
            };

            return Connection.QuerySingleOrDefault<ApplicationTableEntity>(sql.ToString(), param, commandTimeout: DBSettings.CommandTimeout);
        }
    }
}
