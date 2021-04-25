using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaterTrans.DailyReport.Application.TableEntities
{
    /// <summary>
    /// アカウントテーブルエンティティ
    /// </summary>
    [Table("Account")]
    public class AccountTableEntity : SqlTableEntity
    {
        /// <summary>
        /// アカウントID
        /// </summary>
        [Key]
        public Guid AccountId { get; set; }

        /// <summary>
        /// 従業員ID
        /// </summary>
        public Guid PersonId { get; set; }

        /// <summary>
        /// ロール
        /// </summary>
        public string Roles { get; set; }

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
