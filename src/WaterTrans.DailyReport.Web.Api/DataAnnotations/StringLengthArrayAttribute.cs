using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.DataAnnotations
{
    /// <summary>
    /// 文字列配列の文字列長について仕様を定義します。
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class StringLengthArrayAttribute : StringLengthAttribute
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="maximumLength">最大長を指定します。</param>
        public StringLengthArrayAttribute(int maximumLength)
            : base(maximumLength)
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
                return base.IsValid(str);
            }

            return true;
        }
    }
}
