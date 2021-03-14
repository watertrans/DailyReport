using System;
using System.ComponentModel.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.DataAnnotations
{
    /// <summary>
    /// 文字列が許可される値の一覧に含まれているかどうかを定義します。
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class EnumContainsAttribute : AdapteredValidationAttribute
    {
        private readonly EnumDataTypeAttribute _enumDataType;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="enumType">列挙。</param>
        /// <param name="enums">許可される値の一覧。</param>
        public EnumContainsAttribute(Type enumType, params object[] enums)
        {
            _enumDataType = new EnumDataTypeAttribute(enumType);
            Enums = enums;
        }

        /// <summary>
        /// 許可される値の一覧。
        /// </summary>
        public object[] Enums { get; set; }

        /// <inheritdoc/>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (!_enumDataType.IsValid(value))
            {
                return false;
            }

            string stringValue = value as string;
            var convertedValue = stringValue != null
                        ? Enum.Parse(_enumDataType.EnumType, stringValue, false)
                        : Enum.ToObject(_enumDataType.EnumType, value);

            foreach (var item in Enums)
            {
                if (Enum.Equals(item, convertedValue))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
