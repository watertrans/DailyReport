using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WaterTrans.DailyReport.Web.Api.RequestObjects
{
    /// <summary>
    /// トークン作成リクエスト
    /// </summary>
    public class TokenCreateRequest
    {
        /// <summary>
        /// 権限種別
        /// </summary>
        [Display(Name = "DisplayCommonGrantType")]
        [Required(ErrorMessage = "DataAnnotationRequired")]
        [ModelBinder(Name = "grant_type")]
        public string GrantType { get; set; }

        /// <summary>
        /// スコープ
        /// </summary>
        [ModelBinder(Name = "scope")]
        public string Scope { get; set; }

        /// <summary>
        /// クライアントID
        /// </summary>
        [ModelBinder(Name = "client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// クライアントシークレット
        /// </summary>
        [ModelBinder(Name = "client_secret")]
        public string ClientSecret { get; set; }
    }
}
