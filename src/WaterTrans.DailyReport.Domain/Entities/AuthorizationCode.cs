using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Domain.Constants;

namespace WaterTrans.DailyReport.Domain.Entities
{
    /// <summary>
    /// 認可コードエンティティ
    /// </summary>
    public class AuthorizationCode
    {
        /// <summary>
        /// アクセストークンID
        /// </summary>
        public string CodeId { get; set; }

        /// <summary>
        /// アプリケーションID
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// アカウントID
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public AuthorizationCodeStatus Status { get; set; }

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
