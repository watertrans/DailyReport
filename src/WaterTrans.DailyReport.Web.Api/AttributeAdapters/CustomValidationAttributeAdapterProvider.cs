using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using WaterTrans.DailyReport.Web.Api.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.AttributeAdapters
{
    /// <summary>
    /// CustomValidationAttributeAdapterProvider
    /// </summary>
    public class CustomValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

        /// <inheritdoc/>
        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute is AdapteredValidationAttribute)
            {
                return new AdapteredValidationAttributeAdapter(attribute as AdapteredValidationAttribute, stringLocalizer);
            }
            else if (attribute is EnumDataTypeAttribute)
            {
                return new EnumDataTypeAttributeAdapter(attribute as EnumDataTypeAttribute, stringLocalizer);
            }
            else
            {
                return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
            }
        }
    }
}
