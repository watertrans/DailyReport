using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Domain.Constants;

namespace WaterTrans.DailyReport.Domain.Entities
{
    /// <summary>
    /// プロジェクト
    /// </summary>
    public class Project
    {
        /// <summary>
        /// プロジェクトID
        /// </summary>
        public Guid ProjectId { get; set; }

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
        public ProjectStatus Status { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo { get; set; }

        /// <summary>
        /// 所属従業員
        /// </summary>
        public List<Person> Persons { get; set; } = new List<Person>();

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
