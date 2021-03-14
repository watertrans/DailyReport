using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaterTrans.DailyReport.Application.TableEntities
{
    /// <summary>
    /// アクセストークンテーブルエンティティ
    /// </summary>
    [Table("AccessToken")]
    public class AccessTokenTableEntity : SqlTableEntity
    {
        /// <summary>
        /// アクセストークンID
        /// </summary>
        [Key]
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
        /// スコープ
        /// </summary>
        public string Scopes { get; set; }

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

        /// <summary>
        /// 削除日時
        /// </summary>
        public DateTimeOffset? DeleteTime { get; set; }
    }
}
