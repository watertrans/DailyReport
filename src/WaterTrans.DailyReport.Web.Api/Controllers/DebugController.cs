using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Persistence;
using WaterTrans.DailyReport.Web.Api.Filters;

namespace WaterTrans.DailyReport.Web.Api.Controllers
{
    /// <summary>
    /// デバッグコントローラー
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    public class DebugController : ControllerBase
    {
        private readonly ILogger<DebugController> _logger;
        private readonly IDBSettings _dbSettings;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public DebugController(ILogger<DebugController> logger, IDBSettings dbSettings)
        {
            _logger = logger;
            _dbSettings = dbSettings;
        }

        /// <summary>
        /// データベースを初期化する
        /// </summary>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/debug/database/initialize")]
        [SwaggerOperationFilter(typeof(AnonymousOperationFilter))]
        public IActionResult Initialize()
        {
            var setup = new DataSetup(_dbSettings);
            setup.Initialize();
            return new OkResult();
        }

        /// <summary>
        /// データベースの初期データをロードする
        /// </summary>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/debug/database/loadInitialData")]
        [SwaggerOperationFilter(typeof(AnonymousOperationFilter))]
        public IActionResult LoadInitialData()
        {
            var setup = new DataSetup(_dbSettings);
            setup.LoadInitialData();
            return new OkResult();
        }

        /// <summary>
        /// データベースをクリーンアップする
        /// </summary>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/debug/database/cleanup")]
        [SwaggerOperationFilter(typeof(AnonymousOperationFilter))]
        public IActionResult Cleanup()
        {
            var setup = new DataSetup(_dbSettings);
            setup.Cleanup();
            return new OkResult();
        }
    }
}
