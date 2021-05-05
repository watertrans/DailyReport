using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 業務分類クエリーサービスインターフェース
    /// </summary>
    public interface IWorkTypeQueryService
    {
        /// <summary>
        /// 業務分類を検索します。
        /// </summary>
        /// <param name="query">キーワードを指定します。</param>
        /// <param name="sort"><see cref="SortOrder"/></param>
        /// <param name="paging">ページ情報を指定します。</param>
        /// <returns>業務分類の一覧を返します。</returns>
        IList<WorkType> Query(string query, SortOrder sort, PagingQuery paging);

        /// <summary>
        /// 業務分類エンティティを取得します。
        /// </summary>
        /// <param name="workTypeId">プライマリキーを指定します。</param>
        /// <returns>エンティティを返します。存在しない場合はnullを返します。</returns>
        WorkType GetWorkType(Guid workTypeId);

        /// <summary>
        /// 業務分類コードが存在するかどうかを取得します。
        /// </summary>
        /// <param name="workTypeCode">業務分類コードを指定します。</param>
        /// <returns>存在する場合はtrueを返します。</returns>
        bool ExistsWorkTypeCode(string workTypeCode);

        /// <summary>
        /// 業務分類階層が存在するかどうかを取得します。
        /// </summary>
        /// <param name="workTypeTree">業務分類階層を指定します。</param>
        /// <returns>存在する場合はtrueを返します。</returns>
        bool ExistsWorkTypeTree(string workTypeTree);
    }
}
