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
    /// 部署エンドポイントテスト
    /// </summary>
    [TestClass]
    public class GroupTest
    {
        private readonly HttpClient _httpclient;

        public GroupTest()
        {
            _httpclient = TestEnvironment.WebApiFactory.CreateClient();
        }

        [TestMethod]
        public void QueryGroup_OK_読み取りアクセストークンを指定すると正常な応答が返る()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/groups");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void QueryGroup_OK_Sortの指定がGroupCodeの昇順()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "GroupCode" },
                { "query", "部署" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/groups", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Group>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", pagedObject.Items[0].GroupCode);
        }

        [TestMethod]
        public void QueryGroup_OK_Sortの指定がGroupCodeの降順()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "-GroupCode" },
                { "query", "部署" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/groups", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Group>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual("00001", pagedObject.Items[0].GroupCode);
        }

        [TestMethod]
        public void QueryGroup_OK_Sortの指定が複合()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "SortNo,GroupTree,GroupCode,Name,CreateTime" },
                { "query", "部署" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/groups", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Group>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", pagedObject.Items[0].GroupCode);
        }

        [TestMethod]
        public void GetGroup_BadRequest_GroupIDの指定がGuidではない()
        {
            var groupId = "ERROR";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/groups/{groupId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void GetGroup_NotFound_GroupIDの指定が存在しない()
        {
            var groupId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/groups/{groupId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void GetGroup_OK_GroupIDの指定が存在する()
        {
            var groupId = "00000000-3001-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/groups/{groupId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var group = JsonUtil.Deserialize<Group>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", group.GroupCode);
        }

        [TestMethod]
        public void CreateGroup_OK_すべての値が正常値()
        {
            var requestObject = new GroupCreateRequest
            {
                GroupCode = new String('Z', 20),
                GroupTree = new String('Z', 8),
                Name = new String('Z', 256),
                Description = new String('Z', 1024),
                Status = GroupStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('X', 100), new String('Y', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/groups");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var group = JsonUtil.Deserialize<Group>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void CreateGroup_ValidationError_すべての値が異常値()
        {
            var requestObject = new GroupCreateRequest
            {
                GroupCode = new String('Z', 21),
                GroupTree = new String('Z', 9),
                Name = new String('Z', 257),
                Description = new String('Z', 1025),
                Status = "ERROR",
                SortNo = -1,
                Tags = new List<string> { new String('Z', 101) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/groups");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("GroupCode", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("GroupTree", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Name", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Description", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Status", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("SortNo", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Tags", StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void UpdateGroup_BadRequest_GroupIDの指定がGuidではない()
        {
            var groupId = "ERROR";
            var requestObject = new GroupUpdateRequest
            {
                GroupCode = new String('Z', 20),
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/groups/{groupId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void UpdateGroup_Forbidden_書き込みアクセス権のないアクセストークン()
        {
            var groupId = "00000000-0000-0000-0000-000000000000";
            var requestObject = new GroupUpdateRequest
            {
                GroupCode = new String('Z', 20),
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/groups/{groupId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void UpdateGroup_NotFound_GroupIDの指定が存在しない()
        {
            var groupId = "00000000-0000-0000-0000-000000000000";
            var requestObject = new GroupUpdateRequest
            {
                GroupCode = new String('Z', 20),
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/groups/{groupId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void UpdateGroup_OK_すべての値を正常値で更新()
        {
            var requestObject = new GroupCreateRequest
            {
                GroupCode = new String('U', 20),
                GroupTree = new String('U', 8),
                Name = new String('U', 256),
                Description = new String('U', 1024),
                Status = GroupStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('U', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/groups");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var group = JsonUtil.Deserialize<Group>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var updateRequestObject = new GroupUpdateRequest
            {
                GroupCode = new String('V', 20),
                GroupTree = new String('V', 8),
                Name = new String('V', 256),
                Description = new String('V', 1024),
                Status = GroupStatus.SUSPENDED.ToString(),
                SortNo = 0,
                Tags = new List<string> { new String('V', 100) },
            };

            request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/groups/{group.GroupId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(updateRequestObject), Encoding.UTF8, "application/json");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            group = JsonUtil.Deserialize<Group>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(group.GroupCode, new String('V', 20));
            Assert.AreEqual(group.GroupTree, new String('V', 8));
            Assert.AreEqual(group.Name, new String('V', 256));
            Assert.AreEqual(group.Description, new String('V', 1024));
            Assert.AreEqual(group.Status, GroupStatus.SUSPENDED.ToString());
            Assert.AreEqual(group.SortNo, 0);
            Assert.AreEqual(group.Tags[0], new String('V', 100));
        }

        [TestMethod]
        public void UpdateGroup_ValidationError_GroupCodeを重複する値で更新()
        {
            var requestObject = new GroupCreateRequest
            {
                GroupCode = new String('W', 20),
                GroupTree = new String('W', 8),
                Name = new String('W', 256),
                Description = new String('W', 1024),
                Status = GroupStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('W', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/groups");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var group = JsonUtil.Deserialize<Group>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var updateRequestObject = new GroupUpdateRequest
            {
                GroupCode = "00001",
            };

            request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/groups/{group.GroupId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(updateRequestObject), Encoding.UTF8, "application/json");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("GroupCode", StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void UpdateGroup_ValidationError_GroupTreeを重複する値で更新()
        {
            var requestObject = new GroupCreateRequest
            {
                GroupCode = new String('S', 20),
                GroupTree = new String('S', 8),
                Name = new String('S', 256),
                Description = new String('S', 1024),
                Status = GroupStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('S', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/groups");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var group = JsonUtil.Deserialize<Group>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var updateRequestObject = new GroupUpdateRequest
            {
                GroupTree = "01",
            };

            request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/groups/{group.GroupId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(updateRequestObject), Encoding.UTF8, "application/json");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("GroupTree", StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void DeleteGroup_BadRequest_GroupIDの指定がGuidではない()
        {
            var groupId = "ERROR";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/groups/{groupId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void DeleteGroup_Forbidden_書き込みアクセス権のないアクセストークン()
        {
            var groupId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/groups/{groupId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void DeleteGroup_NotFound_GroupIDの指定が存在しない()
        {
            var groupId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/groups/{groupId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void DeleteGroup_ValidationError_所属従業員の存在する部署は削除できない()
        {
            var groupId = "00000000-3001-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/groups/{groupId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [TestMethod]
        public void DeleteGroup_OK_正常値で削除()
        {
            var requestObject = new GroupCreateRequest
            {
                GroupCode = new String('T', 20),
                GroupTree = new String('T', 8),
                Name = new String('X', 256),
                Description = new String('X', 1024),
                Status = GroupStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('X', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/groups");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var group = JsonUtil.Deserialize<Group>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/groups/{group.GroupId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void AddGroupPerson_NotFound_GroupIDの指定が存在しない()
        {
            var groupId = "00000000-0000-0000-0000-000000000000";
            var personId = "00000000-1001-0000-0000-000000000000";
            var requestObject = new GroupPersonAddRequest
            {
                PositionType = PositionType.STAFF.ToString(),
            };

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/groups/{groupId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void AddGroupPerson_NotFound_PersonIDの指定が存在しない()
        {
            var groupId = "00000000-3001-0000-0000-000000000000";
            var personId = "00000000-0000-0000-0000-000000000000";
            var requestObject = new GroupPersonAddRequest
            {
                PositionType = PositionType.STAFF.ToString(),
            };

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/groups/{groupId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void AddGroupPerson_Forbidden_書き込みアクセス権のないアクセストークン()
        {
            var groupId = "00000000-0000-0000-0000-000000000000";
            var personId = "00000000-0000-0000-0000-000000000000";
            var requestObject = new GroupPersonAddRequest
            {
                PositionType = PositionType.STAFF.ToString(),
            };

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/groups/{groupId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void AddGroupPerson_OK_すべての値を正常値で登録()
        {
            var groupId = "00000000-3002-0000-0000-000000000000";
            var personId = "00000000-1001-0000-0000-000000000000";
            var requestObject = new GroupPersonAddRequest
            {
                PositionType = PositionType.GENERAL_MANAGER.ToString(),
            };

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/groups/{groupId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void RemoveGroupPerson_NotFound_GroupIDの指定が存在しない()
        {
            var groupId = "00000000-0000-0000-0000-000000000000";
            var personId = "00000000-1001-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/groups/{groupId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void RemoveGroupPerson_NotFound_PersonIDの指定が存在しない()
        {
            var groupId = "00000000-3001-0000-0000-000000000000";
            var personId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/groups/{groupId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void RemoveGroupPerson_Forbidden_書き込みアクセス権のないアクセストークン()
        {
            var groupId = "00000000-0000-0000-0000-000000000000";
            var personId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/groups/{groupId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void RemoveGroupPerson_OK_すべての値を正常値で削除()
        {
            var groupId = "00000000-3003-0000-0000-000000000000";
            var personId = "00000000-1001-0000-0000-000000000000";
            var requestObject = new GroupPersonAddRequest
            {
                PositionType = PositionType.STAFF.ToString(),
            };

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/groups/{groupId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/groups/{groupId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
