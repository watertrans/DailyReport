using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Persistence.Repositories
{
    /// <summary>
    /// 従業員リポジトリ
    /// </summary>
    public class PersonRepository : SqlRepository<PersonTableEntity>, IPersonRepository
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public PersonRepository(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }
    }
}
