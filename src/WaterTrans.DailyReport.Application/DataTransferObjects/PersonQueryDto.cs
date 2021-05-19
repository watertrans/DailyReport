namespace WaterTrans.DailyReport.Application.DataTransferObjects
{
    /// <summary>
    /// 従業員検索DTO
    /// </summary>
    public class PersonQueryDto : PagingQuery
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// 並び順の指定
        /// </summary>
        public SortOrder Sort { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 部署コード
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// プロジェクトコード
        /// </summary>
        public string ProjectCode { get; set; }
    }
}
