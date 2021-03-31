using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Application.DataTransferObjects;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 部門サービスインターフェース。
    /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// 部門検索
        /// </summary>
        /// <param name="dto"><see cref="GroupQueryDto"/></param>
        /// <returns><see cref="List&lt;Group&gt;"/></returns>
        IList<Group> QueryGroup(GroupQueryDto dto);

        /// <summary>
        /// 部門登録
        /// </summary>
        /// <param name="dto"><see cref="GroupCreateDto"/></param>
        /// <returns><see cref="Group"/></returns>
        Group CreateGroup(GroupCreateDto dto);

        /// <summary>
        /// 部門更新
        /// </summary>
        /// <param name="dto"><see cref="GroupUpdateDto"/></param>
        /// <returns><see cref="Group"/></returns>
        Group UpdateGroup(GroupUpdateDto dto);

        /// <summary>
        /// 部門削除
        /// </summary>
        /// <param name="groupId"><see cref="Guid"/></param>
        void DeleteGroup(Guid groupId);

        /// <summary>
        /// 部門取得
        /// </summary>
        /// <param name="groupId"><see cref="Guid"/></param>
        /// <returns><see cref="Group"/></returns>
        Group GetGroup(Guid groupId);
    }
}
