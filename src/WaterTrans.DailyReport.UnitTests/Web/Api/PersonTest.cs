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
        public void Get_OK_読み取りアクセストークンを指定すると正常な応答が返る()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Get_ValidationError_Pageの指定が数字でない場合はエラー()
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
        public void Get_ValidationError_Pageの指定が1未満の場合はエラー()
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
        public void Get_ValidationError_Pageの指定が1000以上の場合はエラー()
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
        public void Get_OK_Pageの指定が1から999の範囲内の場合は正常()
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
        public void Get_OK_Pageの指定が空欄の場合は正常()
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
        public void Get_ValidationError_PageSizeの指定が数字でない場合はエラー()
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
        public void Get_ValidationError_PageSizeの指定が1未満の場合はエラー()
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
        public void Get_ValidationError_PageSizeの指定が101以上の場合はエラー()
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
        public void Get_OK_PageSizeの指定が1から100の範囲内の場合は正常()
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
        public void Get_OK_PageSizeの指定が空欄の場合は正常()
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
        public void Get_ValidationError_Sortの指定が想定外()
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
        public void Get_OK_Sortの指定がSortNoの昇順()
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
        public void Get_OK_Sortの指定がSortNoの降順()
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
        public void Get_OK_Sortの指定がPersonCodeの昇順()
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
        public void Get_OK_Sortの指定がPersonCodeの降順()
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
        public void Get_OK_Sortの指定がNameの昇順()
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
        public void Get_OK_Sortの指定がNameの降順()
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
        public void Get_OK_Sortの指定がCreateTimeの昇順()
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
        public void Get_OK_Sortの指定がCreateTimeの降順()
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
        public void Get_OK_Sortの指定が複合()
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
        public void Get_BadRequest_PersonIDの指定がGuidではない()
        {
            var personId = "ERROR";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Get_NotFound_PersonIDの指定が存在しない()
        {
            var personId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void Get_OK_PersonIDの指定が存在する()
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
        public void Post_OK_すべての値が正常値()
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
        public void Post_ValidationError_すべての値が異常値()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('Z', 21),
                Name = new String('Z', 101),
                Title = new String('Z', 101),
                Description = new String('Z', 401),
                Status = "ERROR",
                SortNo = -1,
                Tags = new List<string> { new String('Z', 101) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("PersonCode", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Name", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Title", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Description", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Status", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("SortNo", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Tags", StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Post_ValidationError_Tagsの要素が10個を超える()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('Z', 20),
                Name = new String('Z', 100),
                Title = new String('Z', 100),
                Description = new String('Z', 400),
                Status = "NORMAL",
                SortNo = 0,
                Tags = new List<string> {
                    "1","2","3","4","5","6","7","8","9","10","11",
                },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Tags", StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Post_ValidationError_Tagsの要素が空欄()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('Z', 20),
                Name = new String('Z', 100),
                Title = new String('Z', 100),
                Description = new String('Z', 400),
                Status = "NORMAL",
                SortNo = 0,
                Tags = new List<string> {
                    string.Empty,
                },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Tags", StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Post_ValidationError_Tagsの要素がnull()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('Z', 20),
                Name = new String('Z', 100),
                Title = new String('Z', 100),
                Description = new String('Z', 400),
                Status = "NORMAL",
                SortNo = 0,
                Tags = new List<string> {
                    null,
                },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Tags", StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Post_ValidationError_Tagsの要素が重複()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('Z', 20),
                Name = new String('Z', 100),
                Title = new String('Z', 100),
                Description = new String('Z', 400),
                Status = "NORMAL",
                SortNo = 0,
                Tags = new List<string> {
                    "1", "1",
                },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Tags", StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Patch_BadRequest_PersonIDの指定がGuidではない()
        {
            var personId = "ERROR";
            var requestObject = new PersonUpdateRequest
            {
                PersonCode = new String('Z', 20),
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Patch_Forbidden_書き込みアクセス権のないアクセストークン()
        {
            var personId = "00000000-0000-0000-0000-000000000000";
            var requestObject = new PersonUpdateRequest
            {
                PersonCode = new String('Z', 20),
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void Patch_NotFound_PersonIDの指定が存在しない()
        {
            var personId = "00000000-0000-0000-0000-000000000000";
            var requestObject = new PersonUpdateRequest
            {
                PersonCode = new String('Z', 20),
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void Patch_OK_すべての値を正常値で更新()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('U', 20),
                Name = new String('U', 100),
                Title = new String('U', 100),
                Description = new String('U', 400),
                Status = PersonStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('U', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var person = JsonUtil.Deserialize<Person>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var updateRequestObject = new PersonUpdateRequest
            {
                PersonCode = new String('V', 20),
                Name = new String('V', 100),
                Title = new String('V', 100),
                Description = new String('V', 400),
                Status = PersonStatus.SUSPENDED.ToString(),
                SortNo = 0,
                Tags = new List<string> { new String('V', 100) },
            };

            request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/persons/{person.PersonId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(updateRequestObject), Encoding.UTF8, "application/json");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            person = JsonUtil.Deserialize<Person>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(person.PersonCode, new String('V', 20));
            Assert.AreEqual(person.Name, new String('V', 100));
            Assert.AreEqual(person.Title, new String('V', 100));
            Assert.AreEqual(person.Description, new String('V', 400));
            Assert.AreEqual(person.Status, PersonStatus.SUSPENDED.ToString());
            Assert.AreEqual(person.SortNo, 0);
            Assert.AreEqual(person.Tags[0], new String('V', 100));
        }

        [TestMethod]
        public void Patch_ValidationError_重複する値で更新()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('W', 20),
                Name = new String('W', 100),
                Title = new String('W', 100),
                Description = new String('W', 400),
                Status = PersonStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('W', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var person = JsonUtil.Deserialize<Person>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var updateRequestObject = new PersonUpdateRequest
            {
                PersonCode = "00001",
            };

            request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/persons/{person.PersonId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(updateRequestObject), Encoding.UTF8, "application/json");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("PersonCode", StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Delete_BadRequest_PersonIDの指定がGuidではない()
        {
            var personId = "ERROR";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Delete_Forbidden_書き込みアクセス権のないアクセストークン()
        {
            var personId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void Delete_NotFound_PersonIDの指定が存在しない()
        {
            var personId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void Delete_OK_正常値で削除()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('T', 20),
                Name = new String('X', 100),
                Title = new String('X', 100),
                Description = new String('X', 400),
                Status = PersonStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('X', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var person = JsonUtil.Deserialize<Person>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/persons/{person.PersonId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
