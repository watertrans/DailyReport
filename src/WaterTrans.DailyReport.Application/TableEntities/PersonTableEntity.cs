﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaterTrans.DailyReport.Application.TableEntities
{
    /// <summary>
    /// 従業員テーブルエンティティ
    /// </summary>
    [Table("Person")]
    public class PersonTableEntity : SqlTableEntity
    {
        /// <summary>
        /// 従業員ID
        /// </summary>
        [Key]
        public Guid PersonId { get; set; }

        /// <summary>
        /// 従業員コード
        /// </summary>
        public string PersonCode { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 肩書き
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// ソート番号
        /// </summary>
        public int SortNo { get; set; }

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
