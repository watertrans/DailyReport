using System.ComponentModel.DataAnnotations;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Web.Api.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.RequestObjects
{
    /// <summary>
    /// 従業員検索リクエスト
    /// </summary>
    public class PersonQueryRequest
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
        [Sort("SortNo", "PersonCode", "Name", "CreateTime", ErrorMessage = "DataAnnotationSort")]
        public string Sort { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        [Display(Name = "DisplayPersonStatus")]
        [EnumContains(typeof(PersonStatus), PersonStatus.NORMAL, PersonStatus.SUSPENDED, ErrorMessage = "DataAnnotationEnumContains")]
        public string Status { get; set; }

        /// <summary>
        /// 部署コード
        /// </summary>
        [Display(Name = "DisplayGroupGroupCode")]
        [StringLength(20, ErrorMessage = "DataAnnotationStringLength")]
        public string GroupCode { get; set; }

        /// <summary>
        /// プロジェクトコード
        /// </summary>
        [Display(Name = "DisplayProjectProjectCode")]
        [StringLength(20, ErrorMessage = "DataAnnotationStringLength")]
        public string ProjectCode { get; set; }
    }
}
