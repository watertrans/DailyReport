namespace WaterTrans.DailyReport.Application
{
    /// <summary>
    /// 並び順の指定アイテム
    /// </summary>
    public class SortOrderItem
    {
        /// <summary>
        /// 並び順の項目名。
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 昇順または降順。
        /// </summary>
        public SortType SortType { get; set; }
    }
}
