using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace WaterTrans.DailyReport.Application.Utils
{
    /// <summary>
    /// Jsonユーティリティ関数
    /// </summary>
    public static class JsonUtil
    {
        /// <summary>
        /// JsonSerialize および JsonDeserialize メソッドに利用される共通のオプションです。
        /// </summary>
        public static JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions
        {
            // デシリアライズ時に大文字小文字を区別しません
            PropertyNameCaseInsensitive = true,

            // プロパティ名はキャメルケースに変換します
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

            // ディクショナリのキー名はキャメルケースに変換します
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,

            // 列挙型は文字列の値に変換します
            Converters =
            {
                new JsonStringEnumConverter(),
            },

            // 文字参照にしません
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };

        /// <summary>
        /// オブジェクトをJson文字列に変換します。
        /// </summary>
        /// <param name="value">オブジェクトを指定します。</param>
        /// <returns>変換された結果を返します。</returns>
        public static string Serialize(object value)
        {
            if (value == null)
            {
                return null;
            }

            return JsonSerializer.Serialize(value, JsonSerializerOptions);
        }

        /// <summary>
        /// Json文字列をオブジェクトに変換します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型を指定します。</typeparam>
        /// <param name="value">Json文字列を指定します。</param>
        /// <returns>変換された結果を返します。</returns>
        public static T Deserialize<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(value, JsonSerializerOptions);
        }
    }
}
