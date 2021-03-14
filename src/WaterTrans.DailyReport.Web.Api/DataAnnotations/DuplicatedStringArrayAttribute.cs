using System;
using System.Collections.Generic;

namespace WaterTrans.DailyReport.Web.Api.DataAnnotations
{
    /// <summary>
    /// 文字列配列の重複不可を定義します。
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class DuplicatedStringArrayAttribute : AdapteredValidationAttribute
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public DuplicatedStringArrayAttribute()
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

            var list = new List<string>();

            foreach (var str in value as IEnumerable<string>)
            {
                if (list.Contains(str))
                {
                    return false;
                }

                list.Add(str);
            }

            return true;
        }
    }
}
