using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 部署クエリーサービスインターフェース
    /// </summary>
    public interface IGroupQueryService
    {
        /// <summary>
        /// 部署を検索します。
        /// </summary>
        /// <param name="query">キーワードを指定します。</param>
        /// <param name="sort"><see cref="SortOrder"/></param>
        /// <param name="paging">ページ情報を指定します。</param>
        /// <returns>部署の一覧を返します。</returns>
        IList<Group> Query(string query, SortOrder sort, PagingQuery paging);

        /// <summary>
        /// 部署従業員を検索します。
        /// </summary>
        /// <param name="groupId">プライマリキーを指定します。</param>
        /// <param name="query">キーワードを指定します。</param>
        /// <param name="sort"><see cref="SortOrder"/></param>
        /// <param name="paging">ページ情報を指定します。</param>
        /// <returns>部署従業員の一覧を返します。</returns>
        IList<GroupPerson> QueryPerson(Guid groupId, string query, SortOrder sort, PagingQuery paging);

        /// <summary>
        /// 部署エンティティを取得します。
        /// </summary>
        /// <param name="groupId">プライマリキーを指定します。</param>
        /// <returns>エンティティを返します。存在しない場合はnullを返します。</returns>
        Group GetGroup(Guid groupId);

        /// <summary>
        /// 部署コードが存在するかどうかを取得します。
        /// </summary>
        /// <param name="groupCode">部署コードを指定します。</param>
        /// <returns>存在する場合はtrueを返します。</returns>
        bool ExistsGroupCode(string groupCode);

        /// <summary>
        /// 部署階層が存在するかどうかを取得します。
        /// </summary>
        /// <param name="groupTree">部署階層を指定します。</param>
        /// <returns>存在する場合はtrueを返します。</returns>
        bool ExistsGroupTree(string groupTree);

        /// <summary>
        /// すべての部署とマネージャーを取得します。
        /// </summary>
        /// <returns><see cref="List&lt;Group&gt;"/></returns>
        IList<Group> GetOrganization();
    }
}
