using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// アクセストークンリポジトリインターフェース
    /// </summary>
    public interface IAccessTokenRepository : ISqlRepository<AccessTokenTableEntity>
    {
    }
}
