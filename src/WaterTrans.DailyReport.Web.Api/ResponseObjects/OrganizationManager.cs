namespace WaterTrans.DailyReport.Web.Api.ResponseObjects
{
    /// <summary>
    /// 組織マネージャー
    /// </summary>
    public class OrganizationManager
    {
        /// <summary>
        /// 従業員ID
        /// </summary>
        public string PersonId { get; set; }

        /// <summary>
        /// 従業員コード
        /// </summary>
        public string PersonCode { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 役職
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 配属タイプ
        /// </summary>
        public string PositionType { get; set; }
    }
}
