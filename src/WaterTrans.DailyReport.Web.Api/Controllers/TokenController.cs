using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Web.Api.Filters;
using WaterTrans.DailyReport.Web.Api.ObjectResults;
using WaterTrans.DailyReport.Web.Api.RequestObjects;
using WaterTrans.DailyReport.Web.Api.ResponseObjects;

namespace WaterTrans.DailyReport.Web.Api.Controllers
{
    /// <summary>
    /// アクセストークンコントローラー
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    public class TokenController : ControllerBase
    {
        private readonly IAppSettings _appSetgings;
        private readonly IAuthorizeService _authorizeService;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="appSetgings"><see cref="IAppSettings"/></param>
        /// <param name="authorizeService"><see cref="IAuthorizeService"/></param>
        public TokenController(
            IAppSettings appSetgings,
            IAuthorizeService authorizeService)
        {
            _appSetgings = appSetgings;
            _authorizeService = authorizeService;
        }

        /// <summary>
        /// アクセストークンを発行する
        /// </summary>
        /// <param name="request"><see cref="TokenCreateRequest"/></param>
        /// <returns><see cref="ActionResult&lt;Token&gt;"/></returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/token")]
        [Consumes("application/x-www-form-urlencoded")]
        [SwaggerOperationFilter(typeof(AnonymousOperationFilter))]
        public ActionResult<Token> CreateToken([FromForm] TokenCreateRequest request)
        {
            if (request.GrantType == GrantTypes.AuthorizationCode)
            {
                if (string.IsNullOrEmpty(request.ClientId))
                {
                    return ErrorObjectResultFactory.InvalidClient();
                }

                var application = _authorizeService.GetApplication(request.ClientId);

                if (application == null)
                {
                    return ErrorObjectResultFactory.InvalidClient();
                }

                if (!application.GrantTypes.Contains(GrantTypes.AuthorizationCode))
                {
                    return ErrorObjectResultFactory.InvalidGrantType();
                }

                if (string.IsNullOrEmpty(request.Code))
                {
                    return ErrorObjectResultFactory.InvalidCode();
                }

                var authorizationCode = _authorizeService.GetAuthorizationCode(request.Code);

                if (authorizationCode == null)
                {
                    return ErrorObjectResultFactory.InvalidCode();
                }

                if (application.ApplicationId != authorizationCode.ApplicationId)
                {
                    return ErrorObjectResultFactory.InvalidCode();
                }

                var accessToken = _authorizeService.CreateAccessToken(
                    application.ApplicationId,
                    authorizationCode.AccountId,
                    null);

                _authorizeService.UseAuthorizationCode(request.Code);

                return new Token
                {
                    AccessToken = accessToken.TokenId,
                    ExpiresIn = _appSetgings.AccessTokenExpiresIn,
                    Scope = string.Join(' ', accessToken.Scopes),
                    TokenType = "Bearer",
                };
            }
            else if (request.GrantType == GrantTypes.ClientCredentials)
            {
                if (string.IsNullOrEmpty(request.ClientId))
                {
                    return ErrorObjectResultFactory.InvalidClient();
                }

                var application = _authorizeService.GetApplication(request.ClientId);

                if (application == null || application.ClientSecret != request.ClientSecret)
                {
                    return ErrorObjectResultFactory.InvalidClient();
                }

                if (!application.GrantTypes.Contains(GrantTypes.ClientCredentials))
                {
                    return ErrorObjectResultFactory.InvalidGrantType();
                }

                IList<string> scopes = new List<string>();

                if (!string.IsNullOrEmpty(request.Scope))
                {
                    scopes = request.Scope.Split(' ', ',');

                    foreach (string scope in scopes)
                    {
                        if (!application.Scopes.Contains(scope))
                        {
                            return ErrorObjectResultFactory.InvalidScope();
                        }
                    }
                }

                var accessToken = _authorizeService.CreateAccessToken(application.ApplicationId, scopes);

                return new Token
                {
                    AccessToken = accessToken.TokenId,
                    ExpiresIn = _appSetgings.AccessTokenExpiresIn,
                    Scope = string.Join(' ', accessToken.Scopes),
                    TokenType = "Bearer",
                };
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
