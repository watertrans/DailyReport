using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Web.Api.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.RequestObjects
{
    /// <summary>
    /// 部署登録リクエスト
    /// </summary>
    public class GroupCreateRequest
    {
        /// <summary>
        /// 部署コード
        /// </summary>
        [Display(Name = "DisplayGroupGroupCode")]
        [Required(ErrorMessage = "DataAnnotationRequired")]
        [StringLength(20, ErrorMessage = "DataAnnotationStringLength")]
        public string GroupCode { get; set; }

        /// <summary>
        /// 部署階層
        /// </summary>
        [Display(Name = "DisplayGroupGroupTree")]
        [Required(ErrorMessage = "DataAnnotationRequired")]
        [StringLength(8, ErrorMessage = "DataAnnotationStringLength")]
        public string GroupTree { get; set; }

        /// <summary>
        /// 部署名
        /// </summary>
        [Display(Name = "DisplayGroupName")]
        [Required(ErrorMessage = "DataAnnotationRequired")]
        [StringLength(100, ErrorMessage = "DataAnnotationStringLength")]
        public string Name { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        [Display(Name = "DisplayGroupDescription")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "DataAnnotationRequired")]
        [StringLength(400, ErrorMessage = "DataAnnotationStringLength")]
        public string Description { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        [Display(Name = "DisplayGroupStatus")]
        [Required(ErrorMessage = "DataAnnotationRequired")]
        [EnumContains(typeof(GroupStatus), GroupStatus.NORMAL, GroupStatus.SUSPENDED, ErrorMessage = "DataAnnotationEnumContains")]
        public string Status { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Display(Name = "DisplayGroupSortNo")]
        [Required(ErrorMessage = "DataAnnotationRequired")]
        [Range(0, int.MaxValue, ErrorMessage = "DataAnnotationRange")]
        public int SortNo { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        [Display(Name = "DisplayGroupTags")]
        [Required(ErrorMessage = "DataAnnotationRequired")]
        [MaxLength(10, ErrorMessage = "DataAnnotationMaxLength")]
        [StringLengthArray(100, ErrorMessage = "DataAnnotationStringLengthArray")]
        [RequiredStringArray(ErrorMessage = "DataAnnotationRequiredStringArray")]
        [DuplicatedStringArray(ErrorMessage = "DataAnnotationDuplicatedStringArray")]
        public List<string> Tags { get; set; }
    }
}
