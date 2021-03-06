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

namespace WaterTrans.DailyReport.Tests.Web.Api
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
            _httpclient = TestEnvironment.WebApiFactory.CreateClient();
        }

        [TestMethod]
        public void QueryPerson_OK_読み取りアクセストークンを指定すると正常な応答が返る()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/persons");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void QueryPerson_OK_StatusのNORMAL指定に一致した結果が返る()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "status", "NORMAL" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Person>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("NORMAL", pagedObject.Items[0].Status);
        }

        [TestMethod]
        public void QueryPerson_OK_StatusのSUSPENDED指定に一致した結果が返る()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "status", "SUSPENDED" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Person>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("SUSPENDED", pagedObject.Items[0].Status);
        }

        [TestMethod]
        public void QueryPerson_OK_GroupCodeの指定でエラーが出ない()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "groupCode", "00001" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Person>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void QueryPerson_OK_ProjectCodeの指定でエラーが出ない()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "projectCode", "00001" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/persons", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Person>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void QueryPerson_ValidationError_Pageの指定が数字でない場合はエラー()
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
        public void QueryPerson_ValidationError_Pageの指定が1未満の場合はエラー()
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
        public void QueryPerson_ValidationError_Pageの指定が1000以上の場合はエラー()
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
        public void QueryPerson_OK_Pageの指定が1から999の範囲内の場合は正常()
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
        public void QueryPerson_OK_Pageの指定が空欄の場合は正常()
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
        public void QueryPerson_ValidationError_PageSizeの指定が数字でない場合はエラー()
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
        public void QueryPerson_ValidationError_PageSizeの指定が1未満の場合はエラー()
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
        public void QueryPerson_ValidationError_PageSizeの指定が101以上の場合はエラー()
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
        public void QueryPerson_OK_PageSizeの指定が1から100の範囲内の場合は正常()
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
        public void QueryPerson_OK_PageSizeの指定が空欄の場合は正常()
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
        public void QueryPerson_ValidationError_Sortの指定が想定外()
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
        public void QueryPerson_OK_Sortの指定がSortNoの昇順()
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
        public void QueryPerson_OK_Sortの指定がSortNoの降順()
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
        public void QueryPerson_OK_Sortの指定がPersonCodeの昇順()
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
        public void QueryPerson_OK_Sortの指定がPersonCodeの降順()
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
        public void QueryPerson_OK_Sortの指定がNameの昇順()
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
        public void QueryPerson_OK_Sortの指定がNameの降順()
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
        public void QueryPerson_OK_Sortの指定がCreateTimeの昇順()
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
        public void QueryPerson_OK_Sortの指定がCreateTimeの降順()
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
        public void QueryPerson_OK_Sortの指定が複合()
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
        public void GetPerson_BadRequest_PersonIDの指定がGuidではない()
        {
            var personId = "ERROR";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void GetPerson_NotFound_PersonIDの指定が存在しない()
        {
            var personId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void GetPerson_OK_PersonIDの指定が存在する()
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
        public void GetPerson_OK_UserAccessToken_PersonIDの指定が存在する()
        {
            var personId = "00000000-1001-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "user");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var person = JsonUtil.Deserialize<Person>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", person.PersonCode);
        }

        [TestMethod]
        public void CreatePerson_OK_すべての値が正常値()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('Z', 20),
                Name = new String('Z', 256),
                LoginId = new String('Z', 256),
                Title = new String('Z', 100),
                Description = new String('Z', 1024),
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
        public void CreatePerson_ValidationError_すべての値が異常値()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('Z', 21),
                Name = new String('Z', 257),
                LoginId = new String('Z', 257),
                Title = new String('Z', 101),
                Description = new String('Z', 1025),
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
        public void CreatePerson_ValidationError_Tagsの要素が10個を超える()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('Z', 20),
                Name = new String('Z', 256),
                LoginId = new String('Z', 256),
                Title = new String('Z', 100),
                Description = new String('Z', 1024),
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
        public void CreatePerson_ValidationError_Tagsの要素が空欄()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('Z', 20),
                Name = new String('Z', 256),
                LoginId = new String('Z', 256),
                Title = new String('Z', 100),
                Description = new String('Z', 1024),
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
        public void CreatePerson_ValidationError_Tagsの要素がnull()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('Z', 20),
                Name = new String('Z', 256),
                LoginId = new String('Z', 256),
                Title = new String('Z', 100),
                Description = new String('Z', 1024),
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
        public void CreatePerson_ValidationError_Tagsの要素が重複()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('Z', 20),
                Name = new String('Z', 256),
                LoginId = new String('Z', 256),
                Title = new String('Z', 100),
                Description = new String('Z', 1026),
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
        public void UpdatePerson_BadRequest_PersonIDの指定がGuidではない()
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
        public void UpdatePerson_Forbidden_書き込みアクセス権のないアクセストークン()
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
        public void UpdatePerson_NotFound_PersonIDの指定が存在しない()
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
        public void UpdatePerson_OK_すべての値を正常値で更新()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('U', 20),
                Name = new String('U', 256),
                LoginId = new String('U', 256),
                Title = new String('U', 100),
                Description = new String('U', 1024),
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
                Name = new String('V', 256),
                LoginId = new String('V', 256),
                Title = new String('V', 100),
                Description = new String('V', 1024),
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
            Assert.AreEqual(person.Name, new String('V', 256));
            Assert.AreEqual(person.LoginId, new String('V', 256));
            Assert.AreEqual(person.Title, new String('V', 100));
            Assert.AreEqual(person.Description, new String('V', 1024));
            Assert.AreEqual(person.Status, PersonStatus.SUSPENDED.ToString());
            Assert.AreEqual(person.SortNo, 0);
            Assert.AreEqual(person.Tags[0], new String('V', 100));
        }

        [TestMethod]
        public void UpdatePerson_ValidationError_重複する値で更新()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('W', 20),
                Name = new String('W', 256),
                LoginId = new String('W', 256),
                Title = new String('W', 100),
                Description = new String('W', 1024),
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
        public void DeletePerson_BadRequest_PersonIDの指定がGuidではない()
        {
            var personId = "ERROR";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void DeletePerson_Forbidden_書き込みアクセス権のないアクセストークン()
        {
            var personId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void DeletePerson_NotFound_PersonIDの指定が存在しない()
        {
            var personId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void DeletePerson_OK_正常値で削除()
        {
            var requestObject = new PersonCreateRequest
            {
                PersonCode = new String('T', 20),
                Name = new String('X', 256),
                LoginId = new String('T', 256),
                Title = new String('X', 100),
                Description = new String('X', 1024),
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
