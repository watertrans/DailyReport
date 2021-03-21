using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaterTrans.DailyReport.Application.TableEntities
{
    /// <summary>
    /// 作業分類テーブルエンティティ
    /// </summary>
    [Table("WorkType")]
    public class WorkTypeTableEntity : SqlTableEntity
    {
        /// <summary>
        /// 作業分類ID
        /// </summary>
        [Key]
        public Guid WorkTypeId { get; set; }

        /// <summary>
        /// 作業分類コード
        /// </summary>
        public string WorkTypeCode { get; set; }

        /// <summary>
        /// 作業分類階層
        /// </summary>
        public string WorkTypeTree { get; set; }

        /// <summary>
        /// 作業分類名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTimeOffset UpdateTime { get; set; }
    }
}
