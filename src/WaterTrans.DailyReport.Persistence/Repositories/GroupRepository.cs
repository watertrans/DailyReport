using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Persistence.Repositories
{
    /// <summary>
    /// 部署リポジトリ
    /// </summary>
    public class GroupRepository : SqlRepository<GroupTableEntity>, IGroupRepository
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public GroupRepository(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }
    }
}
