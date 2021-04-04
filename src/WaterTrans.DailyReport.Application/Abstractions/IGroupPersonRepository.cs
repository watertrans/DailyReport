using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 部署従業員リポジトリインターフェース
    /// </summary>
    public interface IGroupPersonRepository : ISqlRepository<GroupPersonTableEntity>
    {
    }
}
