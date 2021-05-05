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
    /// 業務分類エンドポイントテスト
    /// </summary>
    [TestClass]
    public class WorkTypeTest
    {
        private readonly HttpClient _httpclient;

        public WorkTypeTest()
        {
            _httpclient = TestEnvironment.WebApiFactory.CreateClient();
        }

        [TestMethod]
        public void QueryWorkType_OK_読み取りアクセストークンを指定すると正常な応答が返る()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/workTypes");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void QueryWorkType_OK_Sortの指定がWorkTypeCodeの昇順()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "WorkTypeCode" },
                { "query", "業務分類" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/workTypes", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<WorkType>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", pagedObject.Items[0].WorkTypeCode);
        }

        [TestMethod]
        public void QueryWorkType_OK_Sortの指定がWorkTypeCodeの降順()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "-WorkTypeCode" },
                { "query", "業務分類" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/workTypes", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<WorkType>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual("00001", pagedObject.Items[0].WorkTypeCode);
        }

        [TestMethod]
        public void QueryWorkType_OK_Sortの指定が複合()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "SortNo,WorkTypeTree,WorkTypeCode,Name,CreateTime" },
                { "query", "業務分類" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/workTypes", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<WorkType>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", pagedObject.Items[0].WorkTypeCode);
        }

        [TestMethod]
        public void GetWorkType_BadRequest_WorkTypeIDの指定がGuidではない()
        {
            var workTypeId = "ERROR";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/workTypes/{workTypeId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void GetWorkType_NotFound_WorkTypeIDの指定が存在しない()
        {
            var workTypeId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/workTypes/{workTypeId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void GetWorkType_OK_WorkTypeIDの指定が存在する()
        {
            var workTypeId = "00000000-4001-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/workTypes/{workTypeId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var workType = JsonUtil.Deserialize<WorkType>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", workType.WorkTypeCode);
        }

        [TestMethod]
        public void CreateWorkType_OK_すべての値が正常値()
        {
            var requestObject = new WorkTypeCreateRequest
            {
                WorkTypeCode = new String('Z', 20),
                WorkTypeTree = new String('9', 8),
                Name = new String('Z', 256),
                Description = new String('Z', 1024),
                Status = WorkTypeStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('X', 100), new String('Y', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/workTypes");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var workType = JsonUtil.Deserialize<WorkType>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void CreateWorkType_ValidationError_すべての値が異常値()
        {
            var requestObject = new WorkTypeCreateRequest
            {
                WorkTypeCode = new String('Z', 21),
                WorkTypeTree = new String('Z', 9),
                Name = new String('Z', 257),
                Description = new String('Z', 1025),
                Status = "ERROR",
                SortNo = -1,
                Tags = new List<string> { new String('Z', 101) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/workTypes");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("WorkTypeCode", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("WorkTypeTree", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Name", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Description", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Status", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("SortNo", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Tags", StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void UpdateWorkType_BadRequest_WorkTypeIDの指定がGuidではない()
        {
            var workTypeId = "ERROR";
            var requestObject = new WorkTypeUpdateRequest
            {
                WorkTypeCode = new String('Z', 20),
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/workTypes/{workTypeId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void UpdateWorkType_Forbidden_書き込みアクセス権のないアクセストークン()
        {
            var workTypeId = "00000000-0000-0000-0000-000000000000";
            var requestObject = new WorkTypeUpdateRequest
            {
                WorkTypeCode = new String('Z', 20),
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/workTypes/{workTypeId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void UpdateWorkType_NotFound_WorkTypeIDの指定が存在しない()
        {
            var workTypeId = "00000000-0000-0000-0000-000000000000";
            var requestObject = new WorkTypeUpdateRequest
            {
                WorkTypeCode = new String('Z', 20),
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/workTypes/{workTypeId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void UpdateWorkType_OK_すべての値を正常値で更新()
        {
            var requestObject = new WorkTypeCreateRequest
            {
                WorkTypeCode = new String('U', 20),
                WorkTypeTree = new String('2', 8),
                Name = new String('U', 256),
                Description = new String('U', 1024),
                Status = WorkTypeStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('U', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/workTypes");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var workType = JsonUtil.Deserialize<WorkType>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var updateRequestObject = new WorkTypeUpdateRequest
            {
                WorkTypeCode = new String('V', 20),
                WorkTypeTree = new String('5', 8),
                Name = new String('V', 256),
                Description = new String('V', 1024),
                Status = WorkTypeStatus.SUSPENDED.ToString(),
                SortNo = 0,
                Tags = new List<string> { new String('V', 100) },
            };

            request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/workTypes/{workType.WorkTypeId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(updateRequestObject), Encoding.UTF8, "application/json");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            workType = JsonUtil.Deserialize<WorkType>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(workType.WorkTypeCode, new String('V', 20));
            Assert.AreEqual(workType.WorkTypeTree, new String('5', 8));
            Assert.AreEqual(workType.Name, new String('V', 256));
            Assert.AreEqual(workType.Description, new String('V', 1024));
            Assert.AreEqual(workType.Status, WorkTypeStatus.SUSPENDED.ToString());
            Assert.AreEqual(workType.SortNo, 0);
            Assert.AreEqual(workType.Tags[0], new String('V', 100));
        }

        [TestMethod]
        public void UpdateWorkType_ValidationError_WorkTypeCodeを重複する値で更新()
        {
            var requestObject = new WorkTypeCreateRequest
            {
                WorkTypeCode = new String('W', 20),
                WorkTypeTree = new String('3', 8),
                Name = new String('W', 256),
                Description = new String('W', 1024),
                Status = WorkTypeStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('W', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/workTypes");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var workType = JsonUtil.Deserialize<WorkType>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var updateRequestObject = new WorkTypeUpdateRequest
            {
                WorkTypeCode = "00001",
            };

            request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/workTypes/{workType.WorkTypeId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(updateRequestObject), Encoding.UTF8, "application/json");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("WorkTypeCode", StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void UpdateWorkType_ValidationError_WorkTypeTreeを重複する値で更新()
        {
            var requestObject = new WorkTypeCreateRequest
            {
                WorkTypeCode = new String('S', 20),
                WorkTypeTree = new String('4', 8),
                Name = new String('S', 256),
                Description = new String('S', 1024),
                Status = WorkTypeStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('S', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/workTypes");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var workType = JsonUtil.Deserialize<WorkType>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var updateRequestObject = new WorkTypeUpdateRequest
            {
                WorkTypeTree = "01",
            };

            request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/workTypes/{workType.WorkTypeId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(updateRequestObject), Encoding.UTF8, "application/json");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("WorkTypeTree", StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void DeleteWorkType_BadRequest_WorkTypeIDの指定がGuidではない()
        {
            var workTypeId = "ERROR";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/workTypes/{workTypeId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void DeleteWorkType_Forbidden_書き込みアクセス権のないアクセストークン()
        {
            var workTypeId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/workTypes/{workTypeId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void DeleteWorkType_NotFound_WorkTypeIDの指定が存在しない()
        {
            var workTypeId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/workTypes/{workTypeId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [TestMethod]
        public void DeleteWorkType_OK_正常値で削除()
        {
            var requestObject = new WorkTypeCreateRequest
            {
                WorkTypeCode = new String('T', 20),
                WorkTypeTree = new String('1', 8),
                Name = new String('X', 256),
                Description = new String('X', 1024),
                Status = WorkTypeStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('X', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/workTypes");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var workType = JsonUtil.Deserialize<WorkType>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/workTypes/{workType.WorkTypeId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
