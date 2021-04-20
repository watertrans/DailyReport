using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Persistence.Repositories
{
    /// <summary>
    /// アカウントリポジトリ
    /// </summary>
    public class AccountRepository : SqlRepository<AccountTableEntity>, IAccountRepository
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public AccountRepository(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }
    }
}
