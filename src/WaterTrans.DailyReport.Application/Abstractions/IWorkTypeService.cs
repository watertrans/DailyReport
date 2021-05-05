using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Application.DataTransferObjects;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 業務分類サービスインターフェース。
    /// </summary>
    public interface IWorkTypeService
    {
        /// <summary>
        /// 業務分類検索
        /// </summary>
        /// <param name="dto"><see cref="WorkTypeQueryDto"/></param>
        /// <returns><see cref="List&lt;WorkType&gt;"/></returns>
        IList<WorkType> QueryWorkType(WorkTypeQueryDto dto);

        /// <summary>
        /// 業務分類登録
        /// </summary>
        /// <param name="dto"><see cref="WorkTypeCreateDto"/></param>
        /// <returns><see cref="WorkType"/></returns>
        WorkType CreateWorkType(WorkTypeCreateDto dto);

        /// <summary>
        /// 業務分類更新
        /// </summary>
        /// <param name="dto"><see cref="WorkTypeUpdateDto"/></param>
        /// <returns><see cref="WorkType"/></returns>
        WorkType UpdateWorkType(WorkTypeUpdateDto dto);

        /// <summary>
        /// 業務分類削除
        /// </summary>
        /// <param name="workTypeId">業務分類ID</param>
        void DeleteWorkType(Guid workTypeId);

        /// <summary>
        /// 業務分類取得
        /// </summary>
        /// <param name="workTypeId">業務分類ID</param>
        /// <returns><see cref="WorkType"/></returns>
        WorkType GetWorkType(Guid workTypeId);
    }
}
