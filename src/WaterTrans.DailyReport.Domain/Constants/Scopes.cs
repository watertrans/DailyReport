namespace WaterTrans.DailyReport.Domain.Constants
{
    /// <summary>
    /// アプリケーションで扱うスコープ一覧
    /// </summary>
    public static class Scopes
    {
        /// <summary>
        /// クレームタイプ
        /// </summary>
        public const string ClaimType = "http://schemas.microsoft.com/identity/claims/scope";

        /// <summary>
        /// 全権
        /// </summary>
        public const string FullControl = "full_control";

        /// <summary>
        /// 読み取り
        /// </summary>
        public const string Read = "read";

        /// <summary>
        /// 書き込み
        /// </summary>
        public const string Write = "write";

        /// <summary>
        /// ユーザー
        /// </summary>
        public const string User = "user";
    }
}
