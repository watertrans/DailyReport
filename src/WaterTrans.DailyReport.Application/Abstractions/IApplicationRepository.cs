using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// アプリケーションリポジトリインターフェース
    /// </summary>
    public interface IApplicationRepository : ISqlRepository<ApplicationTableEntity>
    {
        /// <summary>
        /// クライアントIDで検索します。
        /// </summary>
        /// <param name="clientId">クライアントIDを指定します。</param>
        /// <returns><see cref="ApplicationTableEntity"/></returns>
        ApplicationTableEntity FindByClientId(string clientId);
    }
}
