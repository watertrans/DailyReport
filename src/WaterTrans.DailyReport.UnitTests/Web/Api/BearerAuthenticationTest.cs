﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Web.Api.ObjectResults;
using WaterTrans.DailyReport.Web.Api.ResponseObjects;

namespace WaterTrans.DailyReport.UnitTests.Web.Api
{
    /// <summary>
    /// ベアラー認証テスト
    /// </summary>
    [TestClass]
    public class BearerAuthenticationTest
    {
        private readonly HttpClient _httpclient;

        public BearerAuthenticationTest()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = delegate { return true; };
            var httpclient = new HttpClient(httpClientHandler);
            httpclient.BaseAddress = new Uri(TestEnvironment.WebApiBaseAddress);
            _httpclient = httpclient;
        }

        [TestMethod]
        public void Get_正常_Authorizationを省略するとNoAuthorizationHeader()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/persons");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.NoAuthorizationHeader, error.Code);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_AuthorizationがBearerでないとInvalidAuthorizationScheme()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "YWxhZGRpbjpvcGVuc2VzYW1l");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidAuthorizationScheme, error.Code);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_AuthorizationがBearerでもパラメータが指定されていないとInvalidAuthorizationToken()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidAuthorizationToken, error.Code);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_存在しないアクセストークンを指定するとInvalidAuthorizationToken()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Guid.NewGuid().ToString());
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidAuthorizationToken, error.Code);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_停止されているアクセストークンを指定するとInvalidAuthorizationToken()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "suspended");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InvalidAuthorizationToken, error.Code);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_有効期限切れのアクセストークンを指定するとInvalidAuthorizationToken()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "expired");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.AuthorizationTokenExpired, error.Code);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public void Get_異常_例外が発生するアクセストークンを指定するとInternalServerError()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "exception");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.InternalServerError, error.Code);
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_有効なアクセストークンを指定すると正常な応答が返る()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
