using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// タグリポジトリインターフェース
    /// </summary>
    public interface ITagRepository : ISqlRepository<TagTableEntity>
    {
        /// <summary>
        /// 指定したターゲットIDのタグの一覧を取得します。
        /// </summary>
        /// <param name="targetId">IDを指定します。</param>
        /// <returns>タグの一覧を返します。</returns>
        IList<TagTableEntity> FindByTargetId(Guid targetId);

        /// <summary>
        /// 指定したターゲットIDのタグの一覧を削除します。
        /// </summary>
        /// <param name="targetId">IDを指定します。</param>
        void DeleteByTargetId(Guid targetId);
    }
}
