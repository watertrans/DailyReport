using System.Collections.Generic;

namespace WaterTrans.DailyReport.Application.DataTransferObjects
{
    /// <summary>
    /// 業務分類作成DTO
    /// </summary>
    public class WorkTypeCreateDto
    {
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
    }
}
