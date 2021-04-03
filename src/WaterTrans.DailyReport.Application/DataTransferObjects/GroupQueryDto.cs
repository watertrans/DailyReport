namespace WaterTrans.DailyReport.Application.DataTransferObjects
{
    /// <summary>
    /// 部署検索DTO
    /// </summary>
    public class GroupQueryDto : PagingQuery
    {
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
