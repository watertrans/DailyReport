using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Persistence.Repositories
{
    /// <summary>
    /// 部署従業員リポジトリ
    /// </summary>
    public class GroupPersonRepository : SqlRepository<GroupPersonTableEntity>, IGroupPersonRepository
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public GroupPersonRepository(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }
    }
}
