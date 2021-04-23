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
