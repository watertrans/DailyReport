using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// タグリポジトリインターフェース
    /// </summary>
    public interface ITagRepository : ISqlRepository<TagTableEntity>
    {
    }
}
