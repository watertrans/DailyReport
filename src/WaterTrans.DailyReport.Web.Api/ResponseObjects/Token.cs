using System.Text.Json.Serialization;

namespace WaterTrans.DailyReport.Web.Api.ResponseObjects
{
    /// <summary>
    /// トークン
    /// </summary>
    public class Token
    {
        /// <summary>
        /// アクセストークン
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// アクセストークンの存続時間（秒単位）
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// トークンのタイプ
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// スコープ
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
