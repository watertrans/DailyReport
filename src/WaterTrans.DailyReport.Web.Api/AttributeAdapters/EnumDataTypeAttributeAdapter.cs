using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.AttributeAdapters
{
    /// <summary>
    /// EnumDataTypeAttributeAdapter
    /// </summary>
    public class EnumDataTypeAttributeAdapter : AttributeAdapterBase<EnumDataTypeAttribute>
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="attribute"><see cref="EnumDataTypeAttribute"/></param>
        /// <param name="stringLocalizer"><see cref="IStringLocalizer"/></param>
        public EnumDataTypeAttributeAdapter(EnumDataTypeAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
        }

        /// <inheritdoc/>
        public override void AddValidation(ClientModelValidationContext context)
        {
        }

        /// <inheritdoc/>
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
