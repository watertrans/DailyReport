using System;

namespace WaterTrans.DailyReport.Web.Api.DataAnnotations
{
    /// <summary>
    /// 並び順の指定文字列が許可される値の一覧に含まれているかどうかを定義します。
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class SortAttribute : AdapteredValidationAttribute
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="fields">許可される値の一覧。</param>
        public SortAttribute(params string[] fields)
        {
            Fields = fields;
        }

        /// <summary>
        /// 許可される値の一覧。
        /// </summary>
        public string[] Fields { get; set; }

        /// <inheritdoc/>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (!(value is string))
            {
                return false;
            }

            foreach (string item in ((string)value).Split(','))
            {
                string comparison = item.Trim();
                if (comparison.StartsWith('-'))
                {
                    comparison = comparison.Substring(1).Trim();
                }

                bool found = false;
                foreach (string field in Fields)
                {
                    if (field.Equals(comparison, StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
