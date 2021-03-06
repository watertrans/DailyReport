using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Domain.Constants;

namespace WaterTrans.DailyReport.Domain.Entities
{
    /// <summary>
    /// 業務分類
    /// </summary>
    public class WorkType
    {
        /// <summary>
        /// 業務分類ID
        /// </summary>
        public Guid WorkTypeId { get; set; }

        /// <summary>
        /// 業務分類コード
        /// </summary>
        public string WorkTypeCode { get; set; }

        /// <summary>
        /// 業務分類階層
        /// </summary>
        public string WorkTypeTree { get; set; }

        /// <summary>
        /// 業務分類名
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
        public List<string> Tags { get; set; } = new List<string>();

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
