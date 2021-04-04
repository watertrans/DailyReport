namespace WaterTrans.DailyReport.Web.Api.ResponseObjects
{
    /// <summary>
    /// 部署配属従業員
    /// </summary>
    public class GroupPerson : Person
    {
        /// <summary>
        /// 配属タイプ
        /// </summary>
        public string PositionType { get; set; }
    }
}
