using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.DataAnnotations
{
    /// <summary>
    /// 文字列配列の必須入力を定義します。
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class RequiredStringArrayAttribute : AdapteredValidationAttribute
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public RequiredStringArrayAttribute()
        {
        }

        /// <inheritdoc/>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (!(value is IEnumerable<string>))
            {
                return false;
            }

            foreach (var str in value as IEnumerable<string>)
            {
                if (str == null || str == string.Empty)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
