using System.ComponentModel.DataAnnotations;
using WaterTrans.DailyReport.Web.Api.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.RequestObjects
{
    /// <summary>
    /// 部署検索リクエスト
    /// </summary>
    public class GroupQueryRequest
    {
        /// <summary>
        /// ページ番号
        /// </summary>
        [Display(Name = "DisplayCommonPage")]
        [Range(1, 999, ErrorMessage = "DataAnnotationRange")]
        public int? Page { get; set; }

        /// <summary>
        /// ページサイズ
        /// </summary>
        [Display(Name = "DisplayCommonPageSize")]
        [Range(1, 100, ErrorMessage = "DataAnnotationRange")]
        public int? PageSize { get; set; }

        /// <summary>
        /// 検索条件
        /// </summary>
        [Display(Name = "DisplayCommonQuery")]
        [StringLength(256, ErrorMessage = "DataAnnotationStringLength")]
        public string Query { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Display(Name = "DisplayCommonSort")]
        [StringLength(100, ErrorMessage = "DataAnnotationStringLength")]
        [Sort("SortNo", "GroupCode", "GroupTree", "Name", "CreateTime", ErrorMessage = "DataAnnotationSort")]
        public string Sort { get; set; }
    }
}
