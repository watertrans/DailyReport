using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Web.Api.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.RequestObjects
{
    /// <summary>
    /// 部署更新リクエスト
    /// </summary>
    public class GroupUpdateRequest
    {
        /// <summary>
        /// 部署コード
        /// </summary>
        [Display(Name = "DisplayGroupGroupCode")]
        [RegularExpression(RegExpPatterns.DataCode, ErrorMessage = "DataAnnotationDataCode")]
        [StringLength(20, ErrorMessage = "DataAnnotationStringLength")]
        public string GroupCode { get; set; }

        /// <summary>
        /// 部署階層
        /// </summary>
        [Display(Name = "DisplayGroupGroupTree")]
        [RegularExpression(RegExpPatterns.DataTree, ErrorMessage = "DataAnnotationDataTree")]
        [StringLength(8, ErrorMessage = "DataAnnotationStringLength")]
        public string GroupTree { get; set; }

        /// <summary>
        /// 部署名
        /// </summary>
        [Display(Name = "DisplayGroupName")]
        [StringLength(256, ErrorMessage = "DataAnnotationStringLength")]
        public string Name { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        [Display(Name = "DisplayGroupDescription")]
        [StringLength(1024, ErrorMessage = "DataAnnotationStringLength")]
        public string Description { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        [Display(Name = "DisplayGroupStatus")]
        [EnumContains(typeof(GroupStatus), GroupStatus.NORMAL, GroupStatus.SUSPENDED, ErrorMessage = "DataAnnotationEnumContains")]
        public string Status { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Display(Name = "DisplayGroupSortNo")]
        [Range(0, int.MaxValue, ErrorMessage = "DataAnnotationRange")]
        public int? SortNo { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        [Display(Name = "DisplayGroupTags")]
        [MaxLength(10, ErrorMessage = "DataAnnotationMaxLength")]
        [StringLengthArray(100, ErrorMessage = "DataAnnotationStringLengthArray")]
        [RequiredStringArray(ErrorMessage = "DataAnnotationRequiredStringArray")]
        [DuplicatedStringArray(ErrorMessage = "DataAnnotationDuplicatedStringArray")]
        public List<string> Tags { get; set; }
    }
}
