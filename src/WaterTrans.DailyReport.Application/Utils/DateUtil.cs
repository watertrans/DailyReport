using System;

namespace WaterTrans.DailyReport.Application.Utils
{
    /// <summary>
    /// 日付ユーティリティ関数
    /// </summary>
    public static class DateUtil
    {
        /// <summary>
        /// 現在のタイムゾーンの時刻を取得します。
        /// </summary>
        public static DateTimeOffset Now
        {
            get
            {
                return DateTimeOffset.Now;
            }
        }

        /// <summary>
        /// ISO 8601形式にフォーマットします。
        /// </summary>
        /// <param name="value"><see cref="DateTimeOffset"/></param>
        /// <returns>フォーマット結果を返します。</returns>
        public static string ToISO8601(this DateTimeOffset value)
        {
            return value.ToString("yyyy-MM-ddTHH:mm:sszzz");
        }

        /// <summary>
        /// ISO 8601形式にフォーマットします。
        /// </summary>
        /// <param name="value"><see cref="DateTimeOffset"/></param>
        /// <returns>フォーマット結果を返します。</returns>
        public static string ToISO8601(this DateTimeOffset? value)
        {
            return value?.ToISO8601();
        }
    }
}
