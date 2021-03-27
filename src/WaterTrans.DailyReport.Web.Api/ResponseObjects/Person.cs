using System.Collections.Generic;

namespace WaterTrans.DailyReport.Web.Api.ResponseObjects
{
    /// <summary>
    /// 従業員
    /// </summary>
    public class Person
    {
        /// <summary>
        /// 従業員ID
        /// </summary>
        public string PersonId { get; set; }

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
