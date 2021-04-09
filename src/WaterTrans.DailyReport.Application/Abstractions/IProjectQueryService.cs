using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// プロジェクトクエリーサービスインターフェース
    /// </summary>
    public interface IProjectQueryService
    {
        /// <summary>
        /// プロジェクトを検索します。
        /// </summary>
        /// <param name="query">キーワードを指定します。</param>
        /// <param name="sort"><see cref="SortOrder"/></param>
        /// <param name="paging">ページ情報を指定します。</param>
        /// <returns>プロジェクトの一覧を返します。</returns>
        IList<Project> Query(string query, SortOrder sort, PagingQuery paging);

        /// <summary>
        /// プロジェクト従業員を検索します。
        /// </summary>
        /// <param name="projectId">プライマリキーを指定します。</param>
        /// <param name="query">キーワードを指定します。</param>
        /// <param name="sort"><see cref="SortOrder"/></param>
        /// <param name="paging">ページ情報を指定します。</param>
        /// <returns>プロジェクト従業員の一覧を返します。</returns>
        IList<Person> QueryPerson(Guid projectId, string query, SortOrder sort, PagingQuery paging);

        /// <summary>
        /// プロジェクトエンティティを取得します。
        /// </summary>
        /// <param name="projectId">プライマリキーを指定します。</param>
        /// <returns>エンティティを返します。存在しない場合はnullを返します。</returns>
        Project GetProject(Guid projectId);

        /// <summary>
        /// プロジェクトコードが存在するかどうかを取得します。
        /// </summary>
        /// <param name="projectCode">プロジェクトコードを指定します。</param>
        /// <returns>存在する場合はtrueを返します。</returns>
        bool ExistsProjectCode(string projectCode);
    }
}
