using System;

namespace WaterTrans.DailyReport.Web.Api.DataAnnotations
{
    /// <summary>
    /// 文字列がGUIDかどうかを定義します。
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class GuidAttribute : AdapteredValidationAttribute
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public GuidAttribute()
        {
        }

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
            return Guid.TryParse((string)value, out _);
        }
    }
}
