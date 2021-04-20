using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// アカウントリポジトリインターフェース
    /// </summary>
    public interface IAccountRepository : ISqlRepository<AccountTableEntity>
    {
    }
}
