using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaterTrans.DailyReport.Application.TableEntities
{
    /// <summary>
    /// プロジェクト従業員テーブルエンティティ
    /// </summary>
    [Table("ProjectPerson")]
    public class ProjectPersonTableEntity : SqlTableEntity
    {
        /// <summary>
        /// プロジェクトID
        /// </summary>
        [Key]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 従業員ID
        /// </summary>
        [Key]
        public Guid PersonId { get; set; }
    }
}
