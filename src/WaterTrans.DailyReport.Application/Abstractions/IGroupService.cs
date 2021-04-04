using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Application.DataTransferObjects;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 部署サービスインターフェース。
    /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// 部署検索
        /// </summary>
        /// <param name="dto"><see cref="GroupQueryDto"/></param>
        /// <returns><see cref="List&lt;Group&gt;"/></returns>
        IList<Group> QueryGroup(GroupQueryDto dto);

        /// <summary>
        /// 部署登録
        /// </summary>
        /// <param name="dto"><see cref="GroupCreateDto"/></param>
        /// <returns><see cref="Group"/></returns>
        Group CreateGroup(GroupCreateDto dto);

        /// <summary>
        /// 部署更新
        /// </summary>
        /// <param name="dto"><see cref="GroupUpdateDto"/></param>
        /// <returns><see cref="Group"/></returns>
        Group UpdateGroup(GroupUpdateDto dto);

        /// <summary>
        /// 部署削除
        /// </summary>
        /// <param name="groupId">部署ID</param>
        void DeleteGroup(Guid groupId);

        /// <summary>
        /// 部署取得
        /// </summary>
        /// <param name="groupId">部署ID</param>
        /// <returns><see cref="Group"/></returns>
        Group GetGroup(Guid groupId);

        /// <summary>
        /// 部署従業員に含まれるか確認
        /// </summary>
        /// <param name="groupId">部署ID</param>
        /// <param name="personId">従業員ID</param>
        /// <returns>含まれるかどうか</returns>
        bool ContainsGroupPerson(Guid groupId, Guid personId);

        /// <summary>
        /// 部署従業員配属
        /// </summary>
        /// <param name="groupId">部署ID</param>
        /// <param name="personId">従業員ID</param>
        /// <param name="positionType">配属タイプ</param>
        void AddGroupPerson(Guid groupId, Guid personId, string positionType);

        /// <summary>
        /// 部署従業員解除
        /// </summary>
        /// <param name="groupId">部署ID</param>
        /// <param name="personId">従業員ID</param>
        void RemoveGroupPerson(Guid groupId, Guid personId);
    }
}
