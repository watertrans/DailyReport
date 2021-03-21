﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaterTrans.DailyReport.Application.TableEntities
{
    /// <summary>
    /// 部門テーブルエンティティ
    /// </summary>
    [Table("Group")]
    public class GroupTableEntity : SqlTableEntity
    {
        /// <summary>
        /// 部門ID
        /// </summary>
        [Key]
        public Guid GroupId { get; set; }

        /// <summary>
        /// 部門コード
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 部門階層
        /// </summary>
        public string GroupTree { get; set; }

        /// <summary>
        /// 部門名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 並び順
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
    }
}
