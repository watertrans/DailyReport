using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 認可サービスインターフェース
    /// </summary>
    public interface IAuthorizeService
    {
        /// <summary>
        /// アクセストークンを発行します。
        /// </summary>
        /// <param name="applicationId">アプリケーションIDを指定します。</param>
        /// <param name="scopes">スコープを指定します。nullまたは空を指定する場合はアプリケーションのスコープを採用します。</param>
        /// <returns>アクセストークンの詳細情報を返します。</returns>
        AccessToken CreateAccessToken(Guid applicationId, IList<string> scopes);

        /// <summary>
        /// アクセストークンを発行します。
        /// </summary>
        /// <param name="applicationId">アプリケーションIDを指定します。</param>
        /// <param name="accountId">アカウントIDを指定します。</param>
        /// <param name="scopes">スコープを指定します。nullまたは空を指定する場合はアプリケーションのスコープを採用します。</param>
        /// <returns>アクセストークンの詳細情報を返します。</returns>
        AccessToken CreateAccessToken(Guid applicationId, Guid accountId, IList<string> scopes);

        /// <summary>
        /// 認可コードを発行します。
        /// </summary>
        /// <param name="applicationId">アプリケーションIDを指定します。</param>
        /// <param name="accountId">アカウントIDIDを指定します。</param>
        /// <returns>アクセストークンの詳細情報を返します。</returns>
        AuthorizationCode CreateAuthorizationCode(Guid applicationId, Guid accountId);

        /// <summary>
        /// アクセストークンの詳細情報を取得します。
        /// </summary>
        /// <param name="token">トークンを指定します。</param>
        /// <returns>アクセストークンの詳細情報を返します。アクセストークンが存在しない場合はnullを返します。</returns>
        AccessToken GetAccessToken(string token);

        /// <summary>
        /// アプリケーションの詳細情報を取得します。
        /// </summary>
        /// <param name="clientId">クライアントIDを指定します。</param>
        /// <returns>アプリケーションの詳細情報を返します。アプリケーションが存在しない場合はnullを返します。</returns>
        Domain.Entities.Application GetApplication(string clientId);

        /// <summary>
        /// 認可コードの詳細情報を取得します。
        /// </summary>
        /// <param name="code">認可コードを指定します。</param>
        /// <returns>認可コードの詳細情報を返します。認可コードが存在しないまたは有効期限が切れている場合はnullを返します。</returns>
        Domain.Entities.AuthorizationCode GetAuthorizationCode(string code);

        /// <summary>
        /// 認可コードの使用済みに更新します。
        /// </summary>
        /// <param name="code">認可コードを指定します。</param>
        void UseAuthorizationCode(string code);
    }
}
