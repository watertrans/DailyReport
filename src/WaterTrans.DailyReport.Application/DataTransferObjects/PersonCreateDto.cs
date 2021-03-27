using System.Collections.Generic;

namespace WaterTrans.DailyReport.Application.DataTransferObjects
{
    /// <summary>
    /// 従業員作成DTO
    /// </summary>
    public class PersonCreateDto
    {
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
