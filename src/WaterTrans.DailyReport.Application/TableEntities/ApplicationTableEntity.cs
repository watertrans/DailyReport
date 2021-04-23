using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaterTrans.DailyReport.Application.TableEntities
{
    /// <summary>
    /// アプリケーションテーブルエンティティ
    /// </summary>
    [Table("Application")]
    public class ApplicationTableEntity : SqlTableEntity
    {
        /// <summary>
        /// アプリケーションID
        /// </summary>
        [Key]
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// アプリケーション名
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
        public string Roles { get; set; }

        /// <summary>
        /// スコープ
        /// </summary>
        public string Scopes { get; set; }

        /// <summary>
        /// 権限種別
        /// </summary>
        public string GrantTypes { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

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
