using System.ComponentModel.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.DataAnnotations
{
    /// <summary>
    /// エラーメッセージのローカライズに対応したバリデーションを定義します。
    /// </summary>
    public abstract class AdapteredValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public AdapteredValidationAttribute()
        {
        }
    }
}
