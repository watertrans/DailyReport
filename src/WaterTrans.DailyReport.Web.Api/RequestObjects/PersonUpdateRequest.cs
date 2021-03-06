using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Web.Api.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.RequestObjects
{
    /// <summary>
    /// 従業員更新リクエスト
    /// </summary>
    public class PersonUpdateRequest
    {
        /// <summary>
        /// 従業員コード
        /// </summary>
        [Display(Name = "DisplayPersonPersonCode")]
        [RegularExpression(RegExpPatterns.DataCode, ErrorMessage = "DataAnnotationDataCode")]
        [StringLength(20, ErrorMessage = "DataAnnotationStringLength")]
        public string PersonCode { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        [Display(Name = "DisplayPersonName")]
        [StringLength(256, ErrorMessage = "DataAnnotationStringLength")]
        public string Name { get; set; }

        /// <summary>
        /// ログインID
        /// </summary>
        [Display(Name = "DisplayPersonLoginId")]
        [StringLength(256, ErrorMessage = "DataAnnotationStringLength")]
        public string LoginId { get; set; }

        /// <summary>
        /// 役職
        /// </summary>
        [Display(Name = "DisplayPersonTitle")]
        [StringLength(100, ErrorMessage = "DataAnnotationStringLength")]
        public string Title { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        [Display(Name = "DisplayPersonDescription")]
        [StringLength(1024, ErrorMessage = "DataAnnotationStringLength")]
        public string Description { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        [Display(Name = "DisplayPersonStatus")]
        [EnumContains(typeof(PersonStatus), PersonStatus.NORMAL, PersonStatus.SUSPENDED, ErrorMessage = "DataAnnotationEnumContains")]
        public string Status { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Display(Name = "DisplayPersonSortNo")]
        [Range(0, int.MaxValue, ErrorMessage = "DataAnnotationRange")]
        public int? SortNo { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        [Display(Name = "DisplayPersonTags")]
        [MaxLength(10, ErrorMessage = "DataAnnotationMaxLength")]
        [StringLengthArray(100, ErrorMessage = "DataAnnotationStringLengthArray")]
        [RequiredStringArray(ErrorMessage = "DataAnnotationRequiredStringArray")]
        [DuplicatedStringArray(ErrorMessage = "DataAnnotationDuplicatedStringArray")]
        public List<string> Tags { get; set; }
    }
}
