using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Web.Api.ObjectResults;

namespace WaterTrans.DailyReport.Web.Api.Filters
{
    /// <summary>
    /// デバッグモードでのみ実行可能なコントローラーに適用するフィルター
    /// </summary>
    public class DebugOnlyFilter : ActionFilterAttribute
    {
        /// <inheritdoc/>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var envSettings = filterContext.HttpContext.RequestServices.GetService<IEnvSettings>();
            if (!envSettings.IsDebug)
            {
                filterContext.Result = ErrorObjectResultFactory.Forbidden();
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
