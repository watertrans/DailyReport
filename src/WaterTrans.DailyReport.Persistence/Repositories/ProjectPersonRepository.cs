using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Persistence.Repositories
{
    /// <summary>
    /// プロジェクト従業員リポジトリ
    /// </summary>
    public class ProjectPersonRepository : SqlRepository<ProjectPersonTableEntity>, IProjectPersonRepository
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public ProjectPersonRepository(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }
    }
}
