using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Web.Api.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.RequestObjects
{
    /// <summary>
    /// プロジェクト更新リクエスト
    /// </summary>
    public class ProjectUpdateRequest
    {
        /// <summary>
        /// プロジェクトコード
        /// </summary>
        [Display(Name = "DisplayProjectProjectCode")]
        [RegularExpression(RegExpPatterns.DataCode, ErrorMessage = "DataAnnotationDataCode")]
        [StringLength(20, ErrorMessage = "DataAnnotationStringLength")]
        public string ProjectCode { get; set; }

        /// <summary>
        /// プロジェクト名
        /// </summary>
        [Display(Name = "DisplayProjectName")]
        [StringLength(256, ErrorMessage = "DataAnnotationStringLength")]
        public string Name { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        [Display(Name = "DisplayProjectDescription")]
        [StringLength(1024, ErrorMessage = "DataAnnotationStringLength")]
        public string Description { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        [Display(Name = "DisplayProjectStatus")]
        [EnumContains(typeof(ProjectStatus), ProjectStatus.NORMAL, ProjectStatus.SUSPENDED, ProjectStatus.COMPLETED, ErrorMessage = "DataAnnotationEnumContains")]
        public string Status { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Display(Name = "DisplayProjectSortNo")]
        [Range(0, int.MaxValue, ErrorMessage = "DataAnnotationRange")]
        public int? SortNo { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        [Display(Name = "DisplayProjectTags")]
        [MaxLength(10, ErrorMessage = "DataAnnotationMaxLength")]
        [StringLengthArray(100, ErrorMessage = "DataAnnotationStringLengthArray")]
        [RequiredStringArray(ErrorMessage = "DataAnnotationRequiredStringArray")]
        [DuplicatedStringArray(ErrorMessage = "DataAnnotationDuplicatedStringArray")]
        public List<string> Tags { get; set; }
    }
}
