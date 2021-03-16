using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Persistence.Repositories
{
    /// <summary>
    /// タグリポジトリ
    /// </summary>
    public class TagRepository : SqlRepository<TagTableEntity>, ITagRepository
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public TagRepository(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }
    }
}
