using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 部署リポジトリインターフェース
    /// </summary>
    public interface IGroupRepository : ISqlRepository<GroupTableEntity>
    {
    }
}
