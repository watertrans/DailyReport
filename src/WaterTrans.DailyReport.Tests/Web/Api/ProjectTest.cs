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
    /// プロジェクトエンドポイントテスト
    /// </summary>
    [TestClass]
    public class ProjectTest
    {
        private readonly HttpClient _httpclient;

        public ProjectTest()
        {
            _httpclient = TestEnvironment.WebApiFactory.CreateClient();
        }

        [TestMethod]
        public void QueryProject_OK_読み取りアクセストークンを指定すると正常な応答が返る()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/projects");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void QueryProject_OK_Sortの指定がProjectCodeの昇順()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "ProjectCode" },
                { "query", "プロジェクト" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/projects", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Project>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", pagedObject.Items[0].ProjectCode);
        }

        [TestMethod]
        public void QueryProject_OK_Sortの指定がProjectCodeの降順()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "-ProjectCode" },
                { "query", "プロジェクト" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/projects", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Project>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual("00001", pagedObject.Items[0].ProjectCode);
        }

        [TestMethod]
        public void QueryProject_OK_Sortの指定が複合()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "sort", "SortNo,ProjectCode,Name,CreateTime" },
                { "query", "プロジェクト" },
            };

            var requestUri = QueryHelpers.AddQueryString("/api/v1/projects", parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var pagedObject = JsonUtil.Deserialize<PagedObject<Project>>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", pagedObject.Items[0].ProjectCode);
        }

        [TestMethod]
        public void GetProject_BadRequest_ProjectIDの指定がGuidではない()
        {
            var projectId = "ERROR";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/projects/{projectId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void GetProject_NotFound_ProjectIDの指定が存在しない()
        {
            var projectId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/projects/{projectId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void GetProject_OK_ProjectIDの指定が存在する()
        {
            var projectId = "00000000-2001-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/projects/{projectId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var project = JsonUtil.Deserialize<Project>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("00001", project.ProjectCode);
        }

        [TestMethod]
        public void CreateProject_OK_すべての値が正常値()
        {
            var requestObject = new ProjectCreateRequest
            {
                ProjectCode = new String('Z', 20),
                Name = new String('Z', 256),
                Description = new String('Z', 1024),
                Status = ProjectStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('X', 100), new String('Y', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/projects");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var project = JsonUtil.Deserialize<Project>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void CreateProject_ValidationError_すべての値が異常値()
        {
            var requestObject = new ProjectCreateRequest
            {
                ProjectCode = new String('Z', 21),
                Name = new String('Z', 257),
                Description = new String('Z', 1025),
                Status = "ERROR",
                SortNo = -1,
                Tags = new List<string> { new String('Z', 101) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/projects");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("ProjectCode", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Name", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Description", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Status", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("SortNo", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("Tags", StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void UpdateProject_BadRequest_ProjectIDの指定がGuidではない()
        {
            var projectId = "ERROR";
            var requestObject = new ProjectUpdateRequest
            {
                ProjectCode = new String('Z', 20),
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/projects/{projectId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void UpdateProject_Forbidden_書き込みアクセス権のないアクセストークン()
        {
            var projectId = "00000000-0000-0000-0000-000000000000";
            var requestObject = new ProjectUpdateRequest
            {
                ProjectCode = new String('Z', 20),
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/projects/{projectId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void UpdateProject_NotFound_ProjectIDの指定が存在しない()
        {
            var projectId = "00000000-0000-0000-0000-000000000000";
            var requestObject = new ProjectUpdateRequest
            {
                ProjectCode = new String('Z', 20),
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/projects/{projectId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void UpdateProject_OK_すべての値を正常値で更新()
        {
            var requestObject = new ProjectCreateRequest
            {
                ProjectCode = new String('U', 20),
                Name = new String('U', 256),
                Description = new String('U', 1024),
                Status = ProjectStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('U', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/projects");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var project = JsonUtil.Deserialize<Project>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var updateRequestObject = new ProjectUpdateRequest
            {
                ProjectCode = new String('V', 20),
                Name = new String('V', 256),
                Description = new String('V', 1024),
                Status = ProjectStatus.SUSPENDED.ToString(),
                SortNo = 0,
                Tags = new List<string> { new String('V', 100) },
            };

            request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/projects/{project.ProjectId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(updateRequestObject), Encoding.UTF8, "application/json");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            project = JsonUtil.Deserialize<Project>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(project.ProjectCode, new String('V', 20));
            Assert.AreEqual(project.Name, new String('V', 256));
            Assert.AreEqual(project.Description, new String('V', 1024));
            Assert.AreEqual(project.Status, ProjectStatus.SUSPENDED.ToString());
            Assert.AreEqual(project.SortNo, 0);
            Assert.AreEqual(project.Tags[0], new String('V', 100));
        }

        [TestMethod]
        public void UpdateProject_ValidationError_ProjectCodeを重複する値で更新()
        {
            var requestObject = new ProjectCreateRequest
            {
                ProjectCode = new String('W', 20),
                Name = new String('W', 256),
                Description = new String('W', 1024),
                Status = ProjectStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('W', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/projects");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var project = JsonUtil.Deserialize<Project>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var updateRequestObject = new ProjectUpdateRequest
            {
                ProjectCode = "00001",
            };

            request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/projects/{project.ProjectId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(updateRequestObject), Encoding.UTF8, "application/json");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.IsTrue(error.Details.Exists(e => e.Target.Equals("ProjectCode", StringComparison.OrdinalIgnoreCase)));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void DeleteProject_BadRequest_ProjectIDの指定がGuidではない()
        {
            var projectId = "ERROR";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/projects/{projectId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void DeleteProject_Forbidden_書き込みアクセス権のないアクセストークン()
        {
            var projectId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/projects/{projectId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void DeleteProject_NotFound_ProjectIDの指定が存在しない()
        {
            var projectId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/projects/{projectId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void DeleteProject_ValidationError_所属従業員の存在するプロジェクトは削除できない()
        {
            var projectId = "00000000-2001-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/projects/{projectId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var error = JsonUtil.Deserialize<Error>(responseBody);

            Assert.AreEqual(ErrorCodes.ValidationError, error.Code);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [TestMethod]
        public void DeleteProject_OK_正常値で削除()
        {
            var requestObject = new ProjectCreateRequest
            {
                ProjectCode = new String('T', 20),
                Name = new String('X', 256),
                Description = new String('X', 1024),
                Status = ProjectStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                Tags = new List<string> { new String('X', 100) },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/projects");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            request.Content = new StringContent(JsonUtil.Serialize(requestObject), Encoding.UTF8, "application/json");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var project = JsonUtil.Deserialize<Project>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/projects/{project.ProjectId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void AddProjectPerson_NotFound_ProjectIDの指定が存在しない()
        {
            var projectId = "00000000-0000-0000-0000-000000000000";
            var personId = "00000000-1001-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/projects/{projectId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void AddProjectPerson_NotFound_PersonIDの指定が存在しない()
        {
            var projectId = "00000000-2001-0000-0000-000000000000";
            var personId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/projects/{projectId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void AddProjectPerson_Forbidden_書き込みアクセス権のないアクセストークン()
        {
            var projectId = "00000000-0000-0000-0000-000000000000";
            var personId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/projects/{projectId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void AddProjectPerson_OK_すべての値を正常値で登録()
        {
            var projectId = "00000000-2002-0000-0000-000000000000";
            var personId = "00000000-1001-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/projects/{projectId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void RemoveProjectPerson_NotFound_ProjectIDの指定が存在しない()
        {
            var projectId = "00000000-0000-0000-0000-000000000000";
            var personId = "00000000-1001-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/projects/{projectId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void RemoveProjectPerson_NotFound_PersonIDの指定が存在しない()
        {
            var projectId = "00000000-2001-0000-0000-000000000000";
            var personId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/projects/{projectId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void RemoveProjectPerson_Forbidden_書き込みアクセス権のないアクセストークン()
        {
            var projectId = "00000000-0000-0000-0000-000000000000";
            var personId = "00000000-0000-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/projects/{projectId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-read");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void RemoveProjectPerson_OK_すべての値を正常値で削除()
        {
            var projectId = "00000000-2003-0000-0000-000000000000";
            var personId = "00000000-1001-0000-0000-000000000000";

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/projects/{projectId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            var response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/projects/{projectId}/persons/{personId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "normal-write");
            response = _httpclient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
