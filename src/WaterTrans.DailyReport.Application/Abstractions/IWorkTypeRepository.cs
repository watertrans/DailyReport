using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 作業分類リポジトリインターフェース
    /// </summary>
    public interface IWorkTypeRepository : ISqlRepository<WorkTypeTableEntity>
    {
    }
}
