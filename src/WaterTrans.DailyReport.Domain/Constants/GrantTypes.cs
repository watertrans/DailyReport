namespace WaterTrans.DailyReport.Domain.Constants
{
    /// <summary>
    /// アプリケーションで権限種別一覧
    /// </summary>
    public static class GrantTypes
    {
        /// <summary>
        /// 認可コード
        /// </summary>
        public const string AuthorizationCode = "authorization_code";

        /// <summary>
        /// クライアントクレデンシャル
        /// </summary>
        public const string ClientCredentials = "client_credentials";

        /// <summary>
        /// パスワード
        /// </summary>
        public const string Password = "password";
    }
}
