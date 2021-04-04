using WaterTrans.DailyReport.Domain.Constants;

namespace WaterTrans.DailyReport.Domain.Entities
{
    /// <summary>
    /// 部署配属従業員
    /// </summary>
    public class GroupPerson : Person
    {
        /// <summary>
        /// 配属タイプ
        /// </summary>
        public PositionType PositionType { get; set; }
    }
}
