using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System;
using System.Linq;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.DataTransferObjects;

namespace WaterTrans.DailyReport.Web.Api.Controllers
{
    /// <summary>
    /// ログインコントローラー
    /// </summary>
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="accountService"><see cref="IAccountService"/></param>
        public AccountController(
            IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// ログインページ。
        /// </summary>
        /// <returns><see cref="IActionResult"/></returns>
        public IActionResult Login()
        {
            var accountId = Guid.Parse(User.GetObjectId());

            if (_accountService.ExistsAccount(accountId))
            {
                _accountService.UpdateLastLoginTime(accountId);
            }
            else
            {
                var accountCreateDto = new AccountCreateDto
                {
                    AccountId = accountId,
                    LoginId = User.Identity.Name,
                    Name = User.Claims.ToList().Find(e => e.Type == "name").Value,
                };

                _accountService.CreateAccount(accountCreateDto);
            }

            // TODO 従業員のステータスをチェックして通常であれば認可コードを発行してSPAにリダイレクト
            return this.Content("OK");
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
