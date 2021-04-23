using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaterTrans.DailyReport.Application.TableEntities
{
    /// <summary>
    /// 認可コードテーブルエンティティ
    /// </summary>
    [Table("AuthorizationCode")]
    public class AuthorizationCodeTableEntity : SqlTableEntity
    {
        /// <summary>
        /// 認可コードID
        /// </summary>
        [Key]
        public string CodeId { get; set; }

        /// <summary>
        /// アプリケーションID
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

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
