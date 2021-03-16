using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 部門リポジトリインターフェース
    /// </summary>
    public interface IGroupRepository : ISqlRepository<GroupTableEntity>
    {
    }
}
