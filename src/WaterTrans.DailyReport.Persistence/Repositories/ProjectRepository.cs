using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Persistence.Repositories
{
    /// <summary>
    /// プロジェクトリポジトリ
    /// </summary>
    public class ProjectRepository : SqlRepository<ProjectTableEntity>, IProjectRepository
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public ProjectRepository(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }
    }
}
