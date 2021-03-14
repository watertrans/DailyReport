using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 従業員リポジトリインターフェース
    /// </summary>
    public interface IPersonRepository : ISqlRepository<PersonTableEntity>
    {
    }
}
