using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Domain.Constants;

namespace WaterTrans.DailyReport.Domain.Entities
{
    /// <summary>
    /// アクセストークンエンティティ
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// アクセストークンID
        /// </summary>
        public string TokenId { get; set; }

        /// <summary>
        /// アクセストークン名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// プリンシパルタイプ
        /// </summary>
        public string PrincipalType { get; set; }

        /// <summary>
        /// プリンシパルID
        /// </summary>
        public Guid PrincipalId { get; set; }

        /// <summary>
        /// ロール
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// スコープ
        /// </summary>
        public List<string> Scopes { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public AccessTokenStatus Status { get; set; }

        /// <summary>
        /// 有効期限日時
        /// </summary>
        public DateTimeOffset ExpiryTime { get; set; }

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
