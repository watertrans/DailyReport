using System.Collections.Generic;

namespace WaterTrans.DailyReport.Web.Api.ResponseObjects
{
    /// <summary>
    /// エラー
    /// </summary>
    public class Error : BaseError
    {
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// エラー詳細
        /// </summary>
        public List<Error> Details { get; set; }

        /// <summary>
        /// エラーターゲット
        /// </summary>
        public string Target { get; set; }
    }
}
