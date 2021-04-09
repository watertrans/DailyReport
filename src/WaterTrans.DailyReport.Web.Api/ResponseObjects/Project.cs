using System.Collections.Generic;

namespace WaterTrans.DailyReport.Web.Api.ResponseObjects
{
    /// <summary>
    /// プロジェクト
    /// </summary>
    public class Project
    {
        /// <summary>
        /// プロジェクトID
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// プロジェクトコード
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// プロジェクト名
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
        /// 所属従業員
        /// </summary>
        public List<Person> Persons { get; set; }

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
