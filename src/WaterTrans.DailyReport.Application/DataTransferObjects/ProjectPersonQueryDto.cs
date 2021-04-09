using System;

namespace WaterTrans.DailyReport.Application.DataTransferObjects
{
    /// <summary>
    /// プロジェクト従業員検索DTO
    /// </summary>
    public class ProjectPersonQueryDto : PagingQuery
    {
        /// <summary>
        /// プロジェクトID
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 検索条件
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// 並び順の指定
        /// </summary>
        public SortOrder Sort { get; set; }
    }
}
