using System.Collections.Generic;

namespace WaterTrans.DailyReport.Web.Api.ResponseObjects
{
    /// <summary>
    /// 業務分類
    /// </summary>
    public class WorkType
    {
        /// <summary>
        /// 業務分類ID
        /// </summary>
        public string WorkTypeId { get; set; }

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
        public string Status { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        public string UpdateTime { get; set; }
    }
}
