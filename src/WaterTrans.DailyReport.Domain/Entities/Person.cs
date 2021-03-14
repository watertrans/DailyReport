using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Domain.Constants;

namespace WaterTrans.DailyReport.Domain.Entities
{
    /// <summary>
    /// 従業員
    /// </summary>
    public class Person
    {
        /// <summary>
        /// 従業員ID
        /// </summary>
        public Guid PersonId { get; set; }

        /// <summary>
        /// 従業員コード
        /// </summary>
        public string PersonCode { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 役職
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public PersonStatus Status { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        public int? SortNo { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        public List<Tag> Tags { get; set; } = new List<Tag>();

        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTimeOffset UpdateTime { get; set; }

        /// <summary>
        /// 削除日時
        /// </summary>
        public DateTimeOffset? DeleteTime { get; set; }
    }
}
