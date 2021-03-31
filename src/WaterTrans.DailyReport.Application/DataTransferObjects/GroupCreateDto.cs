using System.Collections.Generic;

namespace WaterTrans.DailyReport.Application.DataTransferObjects
{
    /// <summary>
    /// 部門作成DTO
    /// </summary>
    public class GroupCreateDto
    {
        /// <summary>
        /// 部門コード
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 部門階層
        /// </summary>
        public string GroupTree { get; set; }

        /// <summary>
        /// 部門名
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
