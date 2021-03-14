﻿namespace WaterTrans.DailyReport.Application
{
    /// <summary>
    /// ページングクエリのページ情報
    /// </summary>
    public class PagingQuery
    {
        /// <summary>
        /// ページ番号
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// ページサイズ
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 対象件数
        /// </summary>
        public long TotalCount { get; set; }
    }
}
