using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Persistence.Repositories
{
    /// <summary>
    /// アクセストークンリポジトリ
    /// </summary>
    public class AccessTokenRepository : SqlRepository<AccessTokenTableEntity>, IAccessTokenRepository
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public AccessTokenRepository(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }
    }
}
