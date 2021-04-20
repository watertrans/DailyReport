using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Text;
using System.Threading.Tasks;

namespace WaterTrans.DailyReport.Web.Api.Controllers
{
    /// <summary>
    /// ログインコントローラー
    /// </summary>
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        /// <summary>
        /// ログインページ。
        /// </summary>
        /// <returns><see cref="IActionResult"/></returns>
        public IActionResult Login()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Name: {User.Identity.Name}");
            sb.AppendLine($"ObjectId: {User.GetObjectId()}");
            sb.AppendLine($"NameIdentifierId: {User.GetNameIdentifierId()}");
            sb.AppendLine($"DisplayName: {User.GetDisplayName()}");
            sb.AppendLine($"DomainHint: {User.GetDomainHint()}");
            sb.AppendLine($"HomeObjectId: {User.GetHomeObjectId()}");
            sb.AppendLine($"HomeTenantId: {User.GetHomeTenantId()}");
            sb.AppendLine($"LoginHint: {User.GetLoginHint()}");
            sb.AppendLine($"MsalAccountId: {User.GetMsalAccountId()}");
            sb.AppendLine($"TenantId: {User.GetTenantId()}");
            sb.AppendLine($"UserFlowId: {User.GetUserFlowId()}");

            return this.Content(sb.ToString());
        }

        /// <summary>
        /// ログアウトページ。
        /// </summary>
        /// <returns><see cref="IActionResult"/></returns>
        public IActionResult Logout()
        {
            return Redirect("/MicrosoftIdentity/Account/SignOut");
        }
    }
}
