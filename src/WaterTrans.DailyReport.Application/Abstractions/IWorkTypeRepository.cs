using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 業務分類リポジトリインターフェース
    /// </summary>
    public interface IWorkTypeRepository : ISqlRepository<WorkTypeTableEntity>
    {
    }
}
