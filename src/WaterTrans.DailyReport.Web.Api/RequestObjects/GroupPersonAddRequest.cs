using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Web.Api.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.RequestObjects
{
    /// <summary>
    /// 部署所属従業員登録リクエスト
    /// </summary>
    public class GroupPersonAddRequest
    {
        /// <summary>
        /// 配属タイプ
        /// </summary>
        [Display(Name = "DisplayGroupPersonPositionType")]
        [Required(ErrorMessage = "DataAnnotationRequired")]
        [EnumContains(
            typeof(PositionType),
            Domain.Constants.PositionType.GENERAL_MANAGER,
            Domain.Constants.PositionType.MANAGER,
            Domain.Constants.PositionType.STAFF,
            ErrorMessage = "DataAnnotationEnumContains")]
        public string PositionType { get; set; }
    }
}
