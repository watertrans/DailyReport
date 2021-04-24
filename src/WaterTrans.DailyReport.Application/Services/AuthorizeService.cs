﻿using System;
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

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="appSettings"><see cref="IAppSettings"/></param>
        /// <param name="accessTokenRepository"><see cref="IAccessTokenRepository"/></param>
        /// <param name="authorizationCodeRepository"><see cref="IAuthorizationCodeRepository"/></param>
        /// <param name="applicationRepository"><see cref="IApplicationRepository"/></param>
        public AuthorizeService(
            IAppSettings appSettings,
            IAccessTokenRepository accessTokenRepository,
            IAuthorizationCodeRepository authorizationCodeRepository,
            IApplicationRepository applicationRepository)
        {
            _appSettings = appSettings;
            _accessTokenRepository = accessTokenRepository;
            _authorizationCodeRepository = authorizationCodeRepository;
            _applicationRepository = applicationRepository;
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

            if (accessToken.PrincipalType != PrincipalType.Application.ToString())
            {
                throw new NotImplementedException();
            }

            var application = _applicationRepository.Read(new ApplicationTableEntity { ApplicationId = accessToken.PrincipalId });
            if (application == null || application.Status != ApplicationStatus.NORMAL.ToString())
            {
                return null;
            }

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
    }
}
