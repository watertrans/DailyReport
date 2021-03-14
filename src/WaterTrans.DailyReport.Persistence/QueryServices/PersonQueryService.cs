using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Persistence.QueryServices
{
    /// <summary>
    /// 従業員クエリーサービス
    /// </summary>
    public class PersonQueryService : SqlQueryService, IPersonQueryService
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public PersonQueryService(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }

        /// <inheritdoc/>
        public IList<Person> GetAllPerson()
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT * ");
            sql.AppendLine(" FROM Person ");
            sql.AppendLine(" ORDER By SortNo, PersonCode ");

            var result = Connection.Query<Person>(sql.ToString(), commandTimeout: DBSettings.CommandTimeout);
            return result.ToList();
        }
    }
}
