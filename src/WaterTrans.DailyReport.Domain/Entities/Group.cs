using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Domain.Constants;

namespace WaterTrans.DailyReport.Domain.Entities
{
    /// <summary>
    /// 部署
    /// </summary>
    public class Group
    {
        /// <summary>
        /// 部署ID
        /// </summary>
        public Guid GroupId { get; set; }

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
        public GroupStatus Status { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo { get; set; }

        /// <summary>
        /// 所属従業員
        /// </summary>
        public List<GroupPerson> Persons { get; set; } = new List<GroupPerson>();

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
