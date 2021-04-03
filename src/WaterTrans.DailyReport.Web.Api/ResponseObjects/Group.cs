using System.Collections.Generic;

namespace WaterTrans.DailyReport.Web.Api.ResponseObjects
{
    /// <summary>
    /// 部署
    /// </summary>
    public class Group
    {
        /// <summary>
        /// 部署ID
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 部署コード
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 部署階層
        /// </summary>
        public string GroupTree { get; set; }

        /// <summary>
        /// 部署名
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
