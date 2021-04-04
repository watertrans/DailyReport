using System;

namespace WaterTrans.DailyReport.Application.DataTransferObjects
{
    /// <summary>
    /// 部署従業員検索DTO
    /// </summary>
    public class GroupPersonQueryDto : PagingQuery
    {
        /// <summary>
        /// 部署ID
        /// </summary>
        public Guid GroupId { get; set; }

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
