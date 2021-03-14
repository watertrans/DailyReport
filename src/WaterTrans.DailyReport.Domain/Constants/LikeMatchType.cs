namespace WaterTrans.DailyReport.Domain.Constants
{
    /// <summary>
    /// LIKE検索タイプ
    /// </summary>
    public enum LikeMatchType
    {
        /// <summary>
        /// 前方一致
        /// </summary>
        PrefixSearch = 0,

        /// <summary>
        /// 部分一致
        /// </summary>
        PartialMatch = 1,

        /// <summary>
        /// 後方一致
        /// </summary>
        SuffixSearch = 2,
    }
}
