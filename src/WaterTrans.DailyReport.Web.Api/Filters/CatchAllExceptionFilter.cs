using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using WaterTrans.DailyReport.Web.Api.ObjectResults;

namespace WaterTrans.DailyReport.Web.Api.Filters
{
    /// <summary>
    /// 例外のフィルター
    /// </summary>
    public class CatchAllExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// ILogger
        /// </summary>
        private readonly ILogger<CatchAllExceptionFilter> _logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        public CatchAllExceptionFilter(ILogger<CatchAllExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "InternalServerError");
            context.Result = ErrorObjectResultFactory.InternalServerError();
            context.ExceptionHandled = true;
            return;
        }
    }
}
