using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using WaterTrans.DailyReport.Web.Api.ResponseObjects;

namespace WaterTrans.DailyReport.Web.Api.ObjectResults
{
    /// <summary>
    /// エラー応答
    /// </summary>
    [DefaultStatusCode(DefaultStatusCode)]
    public class ErrorObjectResult : ObjectResult
    {
        private const int DefaultStatusCode = StatusCodes.Status400BadRequest;

        /// <summary>
        /// Creates a new <see cref="ErrorObjectResult"/> instance.
        /// </summary>
        /// <param name="value">The value to format in the entity body.</param>
        public ErrorObjectResult([ActionResultObjectValue] object value)
            : base(value)
        {
            StatusCode = DefaultStatusCode;
        }

        /// <summary>
        /// エラーを取得します。
        /// </summary>
        public Error Error
        {
            get { return (Error)Value; }
        }
    }
}
