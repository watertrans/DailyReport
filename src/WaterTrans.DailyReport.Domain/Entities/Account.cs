using System;
using System.Collections.Generic;

namespace WaterTrans.DailyReport.Domain.Entities
{
    /// <summary>
    /// アカウント
    /// </summary>
    public class Account
    {
        /// <summary>
        /// アカウントID
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// 従業員
        /// </summary>
        public Person Person { get; set; }

        /// <summary>
        /// ロール
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }

        /// <summary>
        /// 最終ログイン日時
        /// </summary>
        public DateTimeOffset LastLoginTime { get; set; }
    }
}
