using System.Collections.Generic;

namespace WaterTrans.DailyReport.Web.Api.ResponseObjects
{
    /// <summary>
    /// ページに分けた検索結果
    /// </summary>
    /// <typeparam name="TObject">結果の型パラメータ</typeparam>
    public class PagedObject<TObject>
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
        public long Total { get; set; }

        /// <summary>
        /// リスト
        /// </summary>
        public List<TObject> Items { get; set; } = new List<TObject>();
    }
}
