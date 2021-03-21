using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Domain.Constants;

namespace WaterTrans.DailyReport.Domain.Entities
{
    /// <summary>
    /// アクセストークンエンティティ
    /// </summary>
    public class Application
    {
        /// <summary>
        /// アクセストークンID
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// アクセストークン名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// クライアントID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// クライアントシークレット
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ロール
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// スコープ
        /// </summary>
        public List<string> Scopes { get; set; }

        /// <summary>
        /// 権限種別
        /// </summary>
        public List<string> GrantTypes { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public ApplicationStatus Status { get; set; }

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
