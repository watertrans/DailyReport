using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Web.Api.ObjectResults;
using WaterTrans.DailyReport.Web.Api.RequestObjects;
using WaterTrans.DailyReport.Web.Api.ResponseObjects;

namespace WaterTrans.DailyReport.UnitTests.Web.Api
{
    /// <summary>
    /// 従業員エンドポイントテスト
    /// </summary>
    [TestClass]
    public class PersonTest
    {
        private readonly HttpClient _httpclient;

        public PersonTest()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = delegate { return true; };
            var httpclient = new HttpClient(httpClientHandler);
            httpclient.BaseAddress = new Uri(TestEnvironment.WebApiBaseAddress);
            _httpclient = httpclient;
        }

        [TestMethod]
        public void Get_正常_読み取りアクセストークンを指定すると正常な応答が返る()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_Pageの指定が数字でない場合はエラー()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "page", "ERROR" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_Pageの指定が1未満の場合はエラー()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "page", "0" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_Pageの指定が1000以上の場合はエラー()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "page", "1000" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_Pageの指定が1から999の範囲内の場合は正常()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "page", "1" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_Pageの指定が空欄の場合は正常()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "page", "" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_PageSizeの指定が数字でない場合はエラー()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "pageSize", "ERROR" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_PageSizeの指定が1未満の場合はエラー()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "pageSize", "0" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_PageSizeの指定が101以上の場合はエラー()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "pageSize", "101" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_PageSizeの指定が1から100の範囲内の場合は正常()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "pageSize", "1" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_PageSizeの指定が空欄の場合は正常()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "pageSize", "" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_Sortの指定が想定外()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "ERROR" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_Sortの指定がSortNoの昇順()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "SortNo" },
                { "query", "従業員" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Person>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", pagedObject.Items[0].PersonCode);
        }

        [TestMethod]
        public void Get_正常_Sortの指定がSortNoの降順()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "-SortNo" },
                { "query", "従業員" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Person>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual("00001", pagedObject.Items[0].PersonCode);
        }

        [TestMethod]
        public void Get_正常_Sortの指定がPersonCodeの昇順()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "PersonCode" },
                { "query", "従業員" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Person>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", pagedObject.Items[0].PersonCode);
        }

        [TestMethod]
        public void Get_正常_Sortの指定がPersonCodeの降順()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "-PersonCode" },
                { "query", "従業員" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Person>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual("00001", pagedObject.Items[0].PersonCode);
        }

        [TestMethod]
        public void Get_正常_Sortの指定がNameの昇順()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "Name" },
                { "query", "従業員" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Person>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", pagedObject.Items[0].PersonCode);
        }

        [TestMethod]
        public void Get_正常_Sortの指定がNameの降順()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "-Name" },
                { "query", "従業員" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Person>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual("00001", pagedObject.Items[0].PersonCode);
        }

        [TestMethod]
        public void Get_正常_Sortの指定がCreateTimeの昇順()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "CreateTime" },
                { "query", "従業員" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Person>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", pagedObject.Items[0].PersonCode);
        }

        [TestMethod]
        public void Get_正常_Sortの指定がCreateTimeの降順()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "-CreateTime" },
                { "query", "従業員" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Person>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual("00001", pagedObject.Items[0].PersonCode);
        }

        [TestMethod]
        public void Get_正常_Sortの指定が複合()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "SortNo,PersonCode,Name,CreateTime" },
                { "query", "従業員" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Person>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", pagedObject.Items[0].PersonCode);
        }

        [TestMethod]
        public void Get_正常_PersonIDの指定がGuidではない()
        {
            var personId = "ERROR";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_PersonIDの指定が存在しない()
        {
            var personId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void Get_正常_PersonIDの指定が存在する()
        {
            var personId = "00000000-1001-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var person = JsonUtil.Deserialize<Person>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", person.PersonCode);
        }

        [TestMethod]
        public void Post_正常_すべての値が正常値()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('Z', 20),
                Name = new String('Z', 100),
                Title = new String('Z', 100),
                Description = new String('Z', 400),
                Status = PersonStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('X', 100), new String('Y', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var person = JsonUtil.Deserialize<Person>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Post_正常_すべての値が異常値()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('Z', 21),
                Name = new String('Z', 101),
                Title = new String('Z', 101),
                Description = new String('Z', 401),
                Status = "ERROR",
                SortNo = -1,
                Tags = new List<string> { null, string.Empty },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.AreEqual(7, error.Details.Count);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        // TODO Patch
        // TODO Delete
    }
}
