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
        /// マネージャーフラグ
        /// </summary>
        [Display(Name = "DisplayGroupPersonIsManager")]
        public bool IsManager { get; set; }
    }
}
