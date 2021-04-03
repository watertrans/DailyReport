using System;

namespace WaterTrans.DailyReport.Application.Utils
{
    /// <summary>
    /// 文字列ユーティリティ関数
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// 文字列をCamelCaseに変換します。
        /// </summary>
        /// <param name="value">文字列を指定します。</param>
        /// <returns>変換した結果を返します。</returns>
        public static string ToCamelCase(this string value)
        {
            if (value == null || value.Length == 0)
            {
                return value;
            }
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        /// <summary>
        /// バイト配列をBase64Urlで文字列に変換します。
        /// </summary>
        /// <param name="input">バイト配列を指定します。</param>
        /// <returns>変換した結果を返します。</returns>
        public static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        /// <summary>
        /// 文字列をBase64Urlでバイト配列に変換します。
        /// </summary>
        /// <param name="input">文字列を指定します。</param>
        /// <returns>変換した結果を返します。</returns>
        public static byte[] Base64UrlDecode(string input)
        {
            string incoming = input.Replace('_', '/').Replace('-', '+');
            switch (input.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }
            return Convert.FromBase64String(incoming);
        }
    }
}
