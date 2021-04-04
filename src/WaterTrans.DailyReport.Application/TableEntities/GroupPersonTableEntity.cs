using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaterTrans.DailyReport.Application.TableEntities
{
    /// <summary>
    /// 部署従業員テーブルエンティティ
    /// </summary>
    [Table("GroupPerson")]
    public class GroupPersonTableEntity : SqlTableEntity
    {
        /// <summary>
        /// 部署ID
        /// </summary>
        [Key]
        public Guid GroupId { get; set; }

        /// <summary>
        /// 従業員ID
        /// </summary>
        [Key]
        public Guid PersonId { get; set; }

        /// <summary>
        /// 配属タイプ
        /// </summary>
        public string PositionType { get; set; }
    }
}
