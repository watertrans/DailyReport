using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Web.Api.ObjectResults;
using WaterTrans.DailyReport.Web.Api.ResponseObjects;

namespace WaterTrans.DailyReport.Tests.Web.Api
{
    /// <summary>
    /// トークンエンドポイントテスト
    /// </summary>
    [TestClass]
    public class TokenTest
    {
        private readonly HttpClient _httpclient;

        public TokenTest()
        {
            _httpclient = TestEnvironment.WebApiFactory.CreateClient();
        }

        [TestMethod]
        public void Post_BadRequest_GrantTypeを省略すると400BadRequest()
        {
            var parameters = new Dictionary<string, string>()
            {
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void Post_InternalServerError_GrantTypeが未実装のものを指定すると例外が発生する()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", "ERROR" },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }

        [TestMethod]
        public void Post_InvalidClient_ClientCredentials_ClientIDを指定しない場合は401InvalidClient()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", GrantTypes.ClientCredentials },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);

            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidClient, error.Code);
        }

        [TestMethod]
        public void Post_InvalidClient_ClientCredentials_アプリケーションが削除済みの場合は401InvalidClient()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", GrantTypes.ClientCredentials },
                { "client_id", "owner-suspended" },
                { "client_secret", "owner-secret" },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);

            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidClient, error.Code);
        }

        [TestMethod]
        public void Post_InvalidClient_ClientCredentials_ClientIDが存在しない場合は401InvalidClient()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", GrantTypes.ClientCredentials },
                { "client_id", "ERROR" },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);

            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidClient, error.Code);
        }

        [TestMethod]
        public void Post_InvalidClient_ClientCredentials_ClientSecretが一致しない場合は401InvalidClient()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", GrantTypes.ClientCredentials },
                { "client_id", "owner-normal" },
                { "client_secret", "ERROR" },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);

            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidClient, error.Code);
        }

        [TestMethod]
        public void Post_InvalidGrantType_ClientCredentials_GrantTypeがClientCredentialsでない場合は400InvalidGrantType()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", GrantTypes.ClientCredentials },
                { "client_id", "password" },
                { "client_secret", "password-secret" },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);

            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidGrantType, error.Code);
        }

        [TestMethod]
        public void Post_InvalidScope_ClientCredentials_Scopeが存在しない場合は400InvalidScope()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", GrantTypes.ClientCredentials },
                { "client_id", "reader-read" },
                { "client_secret", "reader-secret" },
                { "scope", "write" },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);

            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidScope, error.Code);
        }

        [TestMethod]
        public void Post_InvalidClient_AuthorizationCode_ClientIDを指定しない場合は401InvalidClient()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", GrantTypes.AuthorizationCode },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);

            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidClient, error.Code);
        }

        [TestMethod]
        public void Post_InvalidClient_AuthorizationCode_アプリケーションが削除済みの場合は401InvalidClient()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", GrantTypes.AuthorizationCode },
                { "client_id", "clientapp-suspended" },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);

            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidClient, error.Code);
        }

        [TestMethod]
        public void Post_InvalidClient_AuthorizationCode_ClientIDが存在しない場合は401InvalidClient()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", GrantTypes.AuthorizationCode },
                { "client_id", "ERROR" },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);

            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidClient, error.Code);
        }

        [TestMethod]
        public void Post_InvalidGrantType_AuthorizationCode_GrantTypeがAuthorizationCodeでない場合は400InvalidGrantType()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", GrantTypes.AuthorizationCode },
                { "client_id", "owner-normal" },
                { "client_secret", "owner--secret" },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);

            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidGrantType, error.Code);
        }

        [TestMethod]
        public void Post_InvalidCode_AuthorizationCode_Codeを指定しない場合は401InvalidCode()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", GrantTypes.AuthorizationCode },
                { "client_id", "clientapp-normal" },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);

            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidCode, error.Code);
        }

        [TestMethod]
        public void Post_InvalidCode_AuthorizationCode_存在しないCode指定した場合は401InvalidCode()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", GrantTypes.AuthorizationCode },
                { "client_id", "clientapp-normal" },
                { "code", "ERROR" },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);

            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidCode, error.Code);
        }

        [TestMethod]
        public void Post_InvalidCode_AuthorizationCode_認可コード取得時と異なるClientIdを指定した場合は401InvalidCode()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", GrantTypes.AuthorizationCode },
                { "client_id", "clientapp-normal" },
                { "code", "normal2" },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);

            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidCode, error.Code);
        }

        [TestMethod]
        public void Post_OK_ClientCredentials_アクセストークンが発行される()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", GrantTypes.ClientCredentials },
                { "client_id", "owner-normal" },
                { "client_secret", "owner-secret" },
                { "scope", "full_control" },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var token = JsonUtil.Deserialize<Token>(responseBody);

            Assert.AreEqual("Bearer", token.TokenType);
            Assert.AreEqual(3600, token.ExpiresIn);
            Assert.IsNotNull(token.Scope);
            Assert.IsNotNull(token.AccessToken);
        }

        [TestMethod]
        public void Post_OK_AuthorizationCode_アクセストークンが発行される()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", GrantTypes.AuthorizationCode },
                { "client_id", "clientapp-normal" },
                { "code", "normal" },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/token");
            request.Content = new FormUrlEncodedContent(parameters);
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var token = JsonUtil.Deserialize<Token>(responseBody);

            Assert.AreEqual("Bearer", token.TokenType);
            Assert.AreEqual(3600, token.ExpiresIn);
            Assert.IsNotNull(token.Scope);
            Assert.IsNotNull(token.AccessToken);
        }
    }
}
