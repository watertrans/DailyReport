using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Domain.Constants;

namespace WaterTrans.DailyReport.Domain.Entities
{
    /// <summary>
    /// 作業分類
    /// </summary>
    public class WorkType
    {
        /// <summary>
        /// 作業分類ID
        /// </summary>
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
        public WorkTypeStatus Status { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo { get; set; }

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
