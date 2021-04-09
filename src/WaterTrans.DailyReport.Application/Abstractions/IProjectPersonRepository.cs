using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// プロジェクト従業員リポジトリインターフェース
    /// </summary>
    public interface IProjectPersonRepository : ISqlRepository<ProjectPersonTableEntity>
    {
    }
}
