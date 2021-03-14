using WaterTrans.DailyReport.Domain.Constants;

namespace WaterTrans.DailyReport.Persistence
{
    /// <summary>
    /// データアクセスユーティリティ関数
    /// </summary>
    public static class DataUtil
    {
        /// <summary>
        /// LIKE検索用にエスケープ処理を行います。
        /// </summary>
        /// <param name="value">検索文字列を指定します。</param>
        /// <param name="matchType"><see cref="LikeMatchType"/></param>
        /// <returns>エスケープ結果を返します。</returns>
        public static string EscapeLike(string value, LikeMatchType matchType)
        {
            if (value == null)
            {
                return value;
            }

            value = value.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");

            switch (matchType)
            {
                case LikeMatchType.PrefixSearch:
                    return value + "%";
                case LikeMatchType.PartialMatch:
                    return "%" + value + "%";
                case LikeMatchType.SuffixSearch:
                    return "%" + value;
                default:
                    return value;
            }
        }
    }
}
