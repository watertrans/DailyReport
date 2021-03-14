using System;

namespace WaterTrans.DailyReport.Domain.Entities
{
    /// <summary>
    /// タグ
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// タグID
        /// </summary>
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
