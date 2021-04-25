using System;
using System.Collections.Generic;
using System.Linq;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.TableEntities;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Application.Services
{
    /// <summary>
    /// 認可サービス
    /// </summary>
    public class AuthorizeService : IAuthorizeService
    {
        private readonly IAppSettings _appSettings;
        private readonly IAccessTokenRepository _accessTokenRepository;
        private readonly IAuthorizationCodeRepository _authorizationCodeRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IAccountService _accountService;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="appSettings"><see cref="IAppSettings"/></param>
        /// <param name="accessTokenRepository"><see cref="IAccessTokenRepository"/></param>
        /// <param name="authorizationCodeRepository"><see cref="IAuthorizationCodeRepository"/></param>
        /// <param name="applicationRepository"><see cref="IApplicationRepository"/></param>
        /// <param name="accountService"><see cref="IAccountService"/></param>
        public AuthorizeService(
            IAppSettings appSettings,
            IAccessTokenRepository accessTokenRepository,
            IAuthorizationCodeRepository authorizationCodeRepository,
            IApplicationRepository applicationRepository,
            IAccountService accountService)
        {
            _appSettings = appSettings;
            _accessTokenRepository = accessTokenRepository;
            _authorizationCodeRepository = authorizationCodeRepository;
            _applicationRepository = applicationRepository;
            _accountService = accountService;
        }

        /// <inheritdoc/>
        public AccessToken CreateAccessToken(Guid applicationId, Guid accountId, IList<string> scopes)
        {
            if (applicationId == null || applicationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(applicationId));
            }

            if (accountId == null || accountId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(accountId));
            }

            var application = _applicationRepository.Read(new ApplicationTableEntity { ApplicationId = applicationId });
            if (application == null || application.Status != ApplicationStatus.NORMAL.ToString())
            {
                throw new InvalidOperationException($"Application '{applicationId}' was not found.");
            }

            var account = _accountService.GetAccount(accountId);
            if (account == null)
            {
                throw new InvalidOperationException($"Account '{accountId}' was not found.");
            }

            if (account.Person == null || account.Person.Status != PersonStatus.NORMAL)
            {
                throw new InvalidOperationException($"Account '{accountId}' was not valid.");
            }

            var roleScopes = GetAccountRoleScopes(account.Roles);
            IList<string> accessTokenScopes = scopes;
            if (scopes == null || scopes.Count() == 0)
            {
                accessTokenScopes = roleScopes;
            }
            else
            {
                foreach (string scope in scopes)
                {
                    if (!roleScopes.Contains(scope))
                    {
                        throw new InvalidOperationException($"Scope '{scope}' was not included in the account role scope.");
                    }
                }
            }

            var now = DateUtil.Now;
            var accessToken = new AccessTokenTableEntity
            {
                TokenId = StringUtil.CreateCode(),
                ApplicationId = applicationId,
                PrincipalType = PrincipalType.User.ToString(),
                PrincipalId = accountId,
                Name = accountId + " - " + now.ToISO8601(),
                Description = string.Empty,
                Scopes = JsonUtil.Serialize(accessTokenScopes),
                Status = AccessTokenStatus.NORMAL.ToString(),
                ExpiryTime = now.AddSeconds(_appSettings.AccessTokenExpiresIn),
                CreateTime = now,
                UpdateTime = now,
            };

            _accessTokenRepository.Create(accessToken);

            var result = new AccessToken
            {
                Name = accessToken.Name,
                Description = accessToken.Description,
                Roles = account.Roles,
                Scopes = JsonUtil.Deserialize<List<string>>(accessToken.Scopes),
                TokenId = accessToken.TokenId,
                ApplicationId = accessToken.ApplicationId,
                PrincipalType = accessToken.PrincipalType,
                PrincipalId = accessToken.PrincipalId,
                ExpiryTime = accessToken.ExpiryTime,
                Status = (AccessTokenStatus)Enum.Parse(typeof(AccessTokenStatus), accessToken.Status),
                CreateTime = accessToken.CreateTime,
                UpdateTime = accessToken.UpdateTime,
            };

            return result;
        }

        /// <inheritdoc/>
        public AccessToken CreateAccessToken(Guid applicationId, IList<string> scopes)
        {
            if (applicationId == null || applicationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(applicationId));
            }

            var application = _applicationRepository.Read(new ApplicationTableEntity { ApplicationId = applicationId });
            if (application == null || application.Status != ApplicationStatus.NORMAL.ToString())
            {
                throw new InvalidOperationException($"Application '{applicationId}' was not found.");
            }

            IList<string> accessTokenScopes = scopes;
            if (scopes == null || scopes.Count() == 0)
            {
                accessTokenScopes = JsonUtil.Deserialize<List<string>>(application.Scopes);
            }
            else
            {
                foreach (string scope in scopes)
                {
                    if (!application.Scopes.Contains(scope))
                    {
                        throw new InvalidOperationException($"Scope '{scope}' was not included in the application scope.");
                    }
                }
            }

            var now = DateUtil.Now;
            var accessToken = new AccessTokenTableEntity
            {
                TokenId = StringUtil.CreateCode(),
                ApplicationId = applicationId,
                PrincipalType = PrincipalType.Application.ToString(),
                PrincipalId = applicationId,
                Name = application.Name + " - " + now.ToISO8601(),
                Description = string.Empty,
                Scopes = JsonUtil.Serialize(accessTokenScopes),
                Status = AccessTokenStatus.NORMAL.ToString(),
                ExpiryTime = now.AddSeconds(_appSettings.AccessTokenExpiresIn),
                CreateTime = now,
                UpdateTime = now,
            };

            _accessTokenRepository.Create(accessToken);

            var result = new AccessToken
            {
                Name = accessToken.Name,
                Description = accessToken.Description,
                Roles = JsonUtil.Deserialize<List<string>>(application.Roles),
                Scopes = JsonUtil.Deserialize<List<string>>(accessToken.Scopes),
                TokenId = accessToken.TokenId,
                ApplicationId = accessToken.ApplicationId,
                PrincipalType = accessToken.PrincipalType,
                PrincipalId = accessToken.PrincipalId,
                ExpiryTime = accessToken.ExpiryTime,
                Status = (AccessTokenStatus)Enum.Parse(typeof(AccessTokenStatus), accessToken.Status),
                CreateTime = accessToken.CreateTime,
                UpdateTime = accessToken.UpdateTime,
            };

            return result;
        }

        /// <inheritdoc/>
        public AuthorizationCode CreateAuthorizationCode(Guid applicationId, Guid accountId)
        {
            if (applicationId == null || applicationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(applicationId));
            }

            if (accountId == null || accountId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(accountId));
            }

            var application = _applicationRepository.Read(new ApplicationTableEntity { ApplicationId = applicationId });
            if (application == null || application.Status != ApplicationStatus.NORMAL.ToString())
            {
                throw new InvalidOperationException($"Application '{applicationId}' was not found.");
            }

            IList<string> accessTokenScopes = JsonUtil.Deserialize<List<string>>(application.Scopes);

            var now = DateUtil.Now;
            var authorizationCode = new AuthorizationCodeTableEntity
            {
                CodeId = StringUtil.CreateCode(),
                ApplicationId = applicationId,
                AccountId = accountId,
                Status = AuthorizationCodeStatus.NORMAL.ToString(),
                ExpiryTime = now.AddSeconds(_appSettings.AuthorizationCodeExpiresIn),
                CreateTime = now,
                UpdateTime = now,
            };

            _authorizationCodeRepository.Create(authorizationCode);

            var result = new AuthorizationCode
            {
                CodeId = authorizationCode.CodeId,
                ApplicationId = authorizationCode.ApplicationId,
                AccountId = authorizationCode.AccountId,
                ExpiryTime = authorizationCode.ExpiryTime,
                Status = (AuthorizationCodeStatus)Enum.Parse(typeof(AuthorizationCodeStatus), authorizationCode.Status),
                CreateTime = authorizationCode.CreateTime,
                UpdateTime = authorizationCode.UpdateTime,
            };

            return result;
        }

        /// <inheritdoc/>
        public AccessToken GetAccessToken(string token)
        {
            if (token == null || token == string.Empty)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var accessToken = _accessTokenRepository.Read(new AccessTokenTableEntity { TokenId = token });
            if (accessToken == null || accessToken.Status != AccessTokenStatus.NORMAL.ToString())
            {
                return null;
            }

            if (accessToken.PrincipalType != PrincipalType.Application.ToString() &&
                accessToken.PrincipalType != PrincipalType.User.ToString())
            {
                throw new NotImplementedException();
            }

            var application = _applicationRepository.Read(new ApplicationTableEntity { ApplicationId = accessToken.ApplicationId });
            if (application == null || application.Status != ApplicationStatus.NORMAL.ToString())
            {
                return null;
            }

            var roles = JsonUtil.Deserialize<List<string>>(application.Roles);
            if (accessToken.PrincipalType == PrincipalType.User.ToString())
            {
                var account = _accountService.GetAccount(accessToken.PrincipalId);
                if (account == null)
                {
                    return null;
                }

                if (account.Person == null || account.Person.Status != PersonStatus.NORMAL)
                {
                    return null;
                }

                roles = account.Roles;
            }

            var result = new AccessToken
            {
                Name = accessToken.Name,
                Description = accessToken.Description,
                Roles = roles,
                Scopes = JsonUtil.Deserialize<List<string>>(accessToken.Scopes),
                TokenId = accessToken.TokenId,
                ApplicationId = accessToken.ApplicationId,
                PrincipalType = accessToken.PrincipalType,
                PrincipalId = accessToken.PrincipalId,
                ExpiryTime = accessToken.ExpiryTime,
                Status = (AccessTokenStatus)Enum.Parse(typeof(AccessTokenStatus), accessToken.Status),
                CreateTime = accessToken.CreateTime,
                UpdateTime = accessToken.UpdateTime,
            };

            return result;
        }

        /// <inheritdoc/>
        public Domain.Entities.Application GetApplication(string clientId)
        {
            if (clientId == null || clientId == string.Empty)
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            var application = _applicationRepository.FindByClientId(clientId);
            if (application == null || application.Status != ApplicationStatus.NORMAL.ToString())
            {
                return null;
            }

            var result = new Domain.Entities.Application
            {
                ApplicationId = application.ApplicationId,
                ClientId = application.ClientId,
                ClientSecret = application.ClientSecret,
                Name = application.Name,
                Description = application.Description,
                Roles = JsonUtil.Deserialize<List<string>>(application.Roles),
                Scopes = JsonUtil.Deserialize<List<string>>(application.Scopes),
                GrantTypes = JsonUtil.Deserialize<List<string>>(application.GrantTypes),
                RedirectUris = JsonUtil.Deserialize<List<string>>(application.RedirectUris),
                PostLogoutRedirectUris = JsonUtil.Deserialize<List<string>>(application.PostLogoutRedirectUris),
                Status = (ApplicationStatus)Enum.Parse(typeof(ApplicationStatus), application.Status),
                CreateTime = application.CreateTime,
                UpdateTime = application.UpdateTime,
            };

            return result;
        }

        /// <inheritdoc/>
        public Domain.Entities.AuthorizationCode GetAuthorizationCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            var authorizationCode = _authorizationCodeRepository.Read(new AuthorizationCodeTableEntity { CodeId = code });
            if (authorizationCode == null || authorizationCode.Status != ApplicationStatus.NORMAL.ToString())
            {
                return null;
            }

            if (authorizationCode.ExpiryTime < DateUtil.Now)
            {
                return null;
            }

            var result = new Domain.Entities.AuthorizationCode
            {
                CodeId = code,
                ApplicationId = authorizationCode.ApplicationId,
                AccountId = authorizationCode.AccountId,
                ExpiryTime = authorizationCode.ExpiryTime,
                Status = (AuthorizationCodeStatus)Enum.Parse(typeof(AuthorizationCodeStatus), authorizationCode.Status),
                CreateTime = authorizationCode.CreateTime,
                UpdateTime = authorizationCode.UpdateTime,
            };

            return result;
        }

        /// <inheritdoc/>
        public void UseAuthorizationCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            var authorizationCode = _authorizationCodeRepository.Read(new AuthorizationCodeTableEntity { CodeId = code });
            authorizationCode.Status = AuthorizationCodeStatus.USED.ToString();
            authorizationCode.UpdateTime = DateUtil.Now;
            _authorizationCodeRepository.Update(authorizationCode);
        }

        private List<string> GetAccountRoleScopes(List<string> roles)
        {
            var result = new List<string>();
            if (roles.Contains(Roles.Owner))
            {
                result.Add(Scopes.FullControl);
                result.Add(Scopes.Write);
                result.Add(Scopes.Read);
                result.Add(Scopes.User);
            }
            else if (roles.Contains(Roles.Contributor))
            {
                result.Add(Scopes.Write);
                result.Add(Scopes.Read);
                result.Add(Scopes.User);
            }
            else if (roles.Contains(Roles.Reader))
            {
                result.Add(Scopes.Read);
                result.Add(Scopes.User);
            }
            else if (roles.Contains(Roles.User))
            {
                result.Add(Scopes.User);
            }

            return result;
        }
    }
}
