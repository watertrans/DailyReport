using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Persistence.Repositories
{
    /// <summary>
    /// 業務分類リポジトリ
    /// </summary>
    public class WorkTypeRepository : SqlRepository<WorkTypeTableEntity>, IWorkTypeRepository
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public WorkTypeRepository(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }
    }
}
