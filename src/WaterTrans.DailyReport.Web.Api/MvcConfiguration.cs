using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterTrans.DailyReport.Web.Api.Resources;

namespace WaterTrans.DailyReport.Web.Api
{
    /// <inheritdoc cref="IConfigureOptions&lt;MvcOptions&gt;"/>
    public class MvcConfiguration : IConfigureOptions<MvcOptions>
    {
        private readonly IStringLocalizer<ErrorMessages> _stringLocalizer;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="stringLocalizer"><see cref="IStringLocalizer"/></param>
        public MvcConfiguration(IStringLocalizer<ErrorMessages> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        /// <inheritdoc/>
        public void Configure(MvcOptions options)
        {
            options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => _stringLocalizer.GetString("ModelBindingValueIsInvalidAccessor", x));
            options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => _stringLocalizer.GetString("ModelBindingValueMustBeANumberAccessor", x));
            options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => _stringLocalizer.GetString("ModelBindingMissingBindRequiredValueAccessor", x));
            options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => _stringLocalizer.GetString("ModelBindingAttemptedValueIsInvalidAccessor", x, y));
            options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => _stringLocalizer.GetString("ModelBindingMissingKeyOrValueAccessor"));
            options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => _stringLocalizer.GetString("ModelBindingUnknownValueIsInvalidAccessor", x));
            options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => _stringLocalizer.GetString("ModelBindingValueMustNotBeNullAccessor", x));
            options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => _stringLocalizer.GetString("ModelBindingNonPropertyAttemptedValueIsInvalidAccessor", x));
            options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => _stringLocalizer.GetString("ModelBindingNonPropertyUnknownValueIsInvalidAccessor"));
            options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => _stringLocalizer.GetString("ModelBindingNonPropertyValueMustBeANumberAccessor"));
            options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => _stringLocalizer.GetString("ModelBindingMissingRequestBodyRequiredValueAccessor"));
        }
    }
}
