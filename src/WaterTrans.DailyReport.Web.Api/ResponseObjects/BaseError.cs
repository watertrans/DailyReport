namespace WaterTrans.DailyReport.Web.Api.ResponseObjects
{
    /// <summary>
    /// 内部エラー
    /// </summary>
    public class BaseError
    {
        /// <summary>
        /// エラーコード
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 内部エラー
        /// </summary>
        public BaseError InnerError { get; set; }
    }
}
