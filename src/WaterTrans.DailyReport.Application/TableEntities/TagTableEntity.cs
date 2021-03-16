using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaterTrans.DailyReport.Application.TableEntities
{
    /// <summary>
    /// タグテーブルエンティティ
    /// </summary>
    [Table("Tag")]
    public class TagTableEntity : SqlTableEntity
    {
        /// <summary>
        /// タグID
        /// </summary>
        [Key]
        public Guid TagId { get; set; }

        /// <summary>
        /// ターゲットID
        /// </summary>
        public Guid TargetId { get; set; }

        /// <summary>
        /// ターゲットテーブル
        /// </summary>
        public string TargetTable { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
    }
}
