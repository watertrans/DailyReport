using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Persistence.Repositories
{
    /// <summary>
    /// 認可コードリポジトリ
    /// </summary>
    public class AuthorizationCodeRepository : SqlRepository<AuthorizationCodeTableEntity>, IAuthorizationCodeRepository
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public AuthorizationCodeRepository(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }
    }
}
