using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Application.DataTransferObjects;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// プロジェクトサービスインターフェース。
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// プロジェクト検索
        /// </summary>
        /// <param name="dto"><see cref="ProjectQueryDto"/></param>
        /// <returns><see cref="List&lt;Project&gt;"/></returns>
        IList<Project> QueryProject(ProjectQueryDto dto);

        /// <summary>
        /// プロジェクト従業員検索
        /// </summary>
        /// <param name="dto"><see cref="ProjectPersonQueryDto"/></param>
        /// <returns><see cref="List&lt;ProjectPerson&gt;"/></returns>
        IList<Person> QueryPerson(ProjectPersonQueryDto dto);

        /// <summary>
        /// プロジェクト登録
        /// </summary>
        /// <param name="dto"><see cref="ProjectCreateDto"/></param>
        /// <returns><see cref="Project"/></returns>
        Project CreateProject(ProjectCreateDto dto);

        /// <summary>
        /// プロジェクト更新
        /// </summary>
        /// <param name="dto"><see cref="ProjectUpdateDto"/></param>
        /// <returns><see cref="Project"/></returns>
        Project UpdateProject(ProjectUpdateDto dto);

        /// <summary>
        /// プロジェクト削除
        /// </summary>
        /// <param name="projectId">プロジェクトID</param>
        void DeleteProject(Guid projectId);

        /// <summary>
        /// プロジェクト取得
        /// </summary>
        /// <param name="projectId">プロジェクトID</param>
        /// <returns><see cref="Project"/></returns>
        Project GetProject(Guid projectId);

        /// <summary>
        /// プロジェクト従業員に含まれるか確認
        /// </summary>
        /// <param name="projectId">プロジェクトID</param>
        /// <param name="personId">従業員ID</param>
        /// <returns>含まれるかどうか</returns>
        bool ContainsProjectPerson(Guid projectId, Guid personId);

        /// <summary>
        /// プロジェクト従業員配属
        /// </summary>
        /// <param name="projectId">プロジェクトID</param>
        /// <param name="personId">従業員ID</param>
        void AddProjectPerson(Guid projectId, Guid personId);

        /// <summary>
        /// プロジェクト従業員解除
        /// </summary>
        /// <param name="projectId">プロジェクトID</param>
        /// <param name="personId">従業員ID</param>
        void RemoveProjectPerson(Guid projectId, Guid personId);
    }
}
