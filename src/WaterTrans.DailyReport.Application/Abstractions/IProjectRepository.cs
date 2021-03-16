using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// プロジェクトリポジトリインターフェース
    /// </summary>
    public interface IProjectRepository : ISqlRepository<ProjectTableEntity>
    {
    }
}
