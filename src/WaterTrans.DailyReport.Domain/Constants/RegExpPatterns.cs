namespace WaterTrans.DailyReport.Domain.Constants
{
    /// <summary>
    /// 正規表現パターン定数
    /// </summary>
    public static class RegExpPatterns
    {
        /// <summary>
        /// コード
        /// </summary>
        public const string DataCode = "^[0-9A-Za-z\\-]*$";

        /// <summary>
        /// 階層
        /// </summary>
        public const string DataTree = "^[0-9]{2}$|^[0-9]{4}$|^[0-9]{6}$|^[0-9]{8}$";
    }
}
