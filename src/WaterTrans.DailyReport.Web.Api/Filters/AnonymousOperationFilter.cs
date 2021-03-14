using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WaterTrans.DailyReport.Web.Api.Filters
{
    /// <summary>
    /// 匿名アクセスドキュメント生成フィルター
    /// </summary>
    public class AnonymousOperationFilter : IOperationFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme(),
                    new string[] { }
                },
            });
        }
    }
}
