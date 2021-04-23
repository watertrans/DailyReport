using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 認可コードリポジトリインターフェース
    /// </summary>
    public interface IAuthorizationCodeRepository : ISqlRepository<AuthorizationCodeTableEntity>
    {
    }
}
