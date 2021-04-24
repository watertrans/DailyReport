using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System;
using System.Linq;
using System.Web;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.DataTransferObjects;
using WaterTrans.DailyReport.Domain.Constants;

namespace WaterTrans.DailyReport.Web.Api.Controllers
{
    /// <summary>
    /// ログインコントローラー
    /// </summary>
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAuthorizeService _authorizetService;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="accountService"><see cref="IAccountService"/></param>
        /// <param name="authorizeService"><see cref="IAuthorizeService"/></param>
        public AccountController(
            IAccountService accountService,
            IAuthorizeService authorizeService)
        {
            _accountService = accountService;
            _authorizetService = authorizeService;
        }

        /// <summary>
        /// ログインページ。
        /// </summary>
        /// <param name="clientId">クライアントID</param>
        /// <param name="redirectUri">リダイレクトURI</param>
        /// <returns><see cref="IActionResult"/></returns>
        public IActionResult Login(string clientId = "clientapp", string redirectUri = null)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return View("LoginClientError");
            }

            var application = _authorizetService.GetApplication(clientId);

            if (application == null || application.Status != ApplicationStatus.NORMAL)
            {
                return View("LoginClientError");
            }

            if (!application.GrantTypes.Contains(GrantTypes.AuthorizationCode))
            {
                return View("LoginGrantTypeError");
            }

            if (application.RedirectUris.Count == 0 ||
                (redirectUri != null && !application.RedirectUris.Exists(e => e.ToLower() == redirectUri.ToLower())))
            {
                return View("LoginRedirectError");
            }

            var accountId = Guid.Parse(User.GetObjectId());
            var account = _accountService.GetAccount(accountId);
            var accountCreateDto = new AccountCreateDto
            {
                AccountId = accountId,
                LoginId = User.Identity.Name,
                Name = User.Claims.ToList().Find(e => e.Type == "name").Value,
            };

            if (account != null && account.Person != null)
            {
                _accountService.UpdateLastLoginTime(accountId);
            }
            else if (account != null && account.Person == null)
            {
                _accountService.RecreateAccountAndPerson(accountCreateDto);
                account = _accountService.GetAccount(accountId);
            }
            else
            {
                _accountService.CreateAccountAndPerson(accountCreateDto);
                account = _accountService.GetAccount(accountId);
            }

            var authorizationCode = _authorizetService.CreateAuthorizationCode(application.ApplicationId, accountId);
            string url = redirectUri ?? application.RedirectUris[0];

            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["code"] = authorizationCode.CodeId;
            uriBuilder.Query = query.ToString();
            url = uriBuilder.Uri.ToString();

            return Redirect(url);
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
