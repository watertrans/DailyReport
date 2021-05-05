using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Web.Api.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.RequestObjects
{
    /// <summary>
    /// 業務分類登録リクエスト
    /// </summary>
    public class WorkTypeCreateRequest
    {
        /// <summary>
        /// 業務分類コード
        /// </summary>
        [Display(Name = "DisplayWorkTypeWorkTypeCode")]
        [Required(ErrorMessage = "DataAnnotationRequired")]
        [RegularExpression(RegExpPatterns.DataCode, ErrorMessage = "DataAnnotationDataCode")]
        [StringLength(20, ErrorMessage = "DataAnnotationStringLength")]
        public string WorkTypeCode { get; set; }

        /// <summary>
        /// 業務分類階層
        /// </summary>
        [Display(Name = "DisplayWorkTypeWorkTypeTree")]
        [Required(ErrorMessage = "DataAnnotationRequired")]
        [RegularExpression(RegExpPatterns.DataTree, ErrorMessage = "DataAnnotationDataTree")]
        [StringLength(8, ErrorMessage = "DataAnnotationStringLength")]
        public string WorkTypeTree { get; set; }

        /// <summary>
        /// 業務分類名
        /// </summary>
        [Display(Name = "DisplayWorkTypeName")]
        [Required(ErrorMessage = "DataAnnotationRequired")]
        [StringLength(256, ErrorMessage = "DataAnnotationStringLength")]
        public string Name { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        [Display(Name = "DisplayWorkTypeDescription")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "DataAnnotationRequired")]
        [StringLength(1024, ErrorMessage = "DataAnnotationStringLength")]
        public string Description { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        [Display(Name = "DisplayWorkTypeStatus")]
        [Required(ErrorMessage = "DataAnnotationRequired")]
        [EnumContains(typeof(WorkTypeStatus), WorkTypeStatus.NORMAL, WorkTypeStatus.SUSPENDED, ErrorMessage = "DataAnnotationEnumContains")]
        public string Status { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Display(Name = "DisplayWorkTypeSortNo")]
        [Required(ErrorMessage = "DataAnnotationRequired")]
        [Range(0, int.MaxValue, ErrorMessage = "DataAnnotationRange")]
        public int SortNo { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        [Display(Name = "DisplayWorkTypeTags")]
        [Required(ErrorMessage = "DataAnnotationRequired")]
        [MaxLength(10, ErrorMessage = "DataAnnotationMaxLength")]
        [StringLengthArray(100, ErrorMessage = "DataAnnotationStringLengthArray")]
        [RequiredStringArray(ErrorMessage = "DataAnnotationRequiredStringArray")]
        [DuplicatedStringArray(ErrorMessage = "DataAnnotationDuplicatedStringArray")]
        public List<string> Tags { get; set; }
    }
}
