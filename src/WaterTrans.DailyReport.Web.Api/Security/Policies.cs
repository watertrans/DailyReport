namespace WaterTrans.DailyReport.Web.Api.Security
{
    /// <summary>
    /// アプリケーションで扱うポリシー一覧
    /// </summary>
    public static class Policies
    {
        /// <summary>
        /// FullControlポリシー
        /// </summary>
        public const string FullControlScopePolicy = "FullControlScopePolicy";

        /// <summary>
        /// Readポリシー
        /// </summary>
        public const string ReadScopePolicy = "ReadScopePolicy";

        /// <summary>
        /// Writeポリシー
        /// </summary>
        public const string WriteScopePolicy = "WriteScopePolicy";
    }
}
