using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using WaterTrans.DailyReport.Web.Api.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.AttributeAdapters
{
    /// <summary>
    /// AdapteredValidationAttributeAdapter
    /// </summary>
    public class AdapteredValidationAttributeAdapter : AttributeAdapterBase<AdapteredValidationAttribute>
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="attribute"><see cref="AdapteredValidationAttribute"/></param>
        /// <param name="stringLocalizer"><see cref="IStringLocalizer"/></param>
        public AdapteredValidationAttributeAdapter(AdapteredValidationAttribute attribute, IStringLocalizer stringLocalizer)
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
