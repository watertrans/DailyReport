using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.DataTransferObjects;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Web.Api.DataAnnotations;
using WaterTrans.DailyReport.Web.Api.ObjectResults;
using WaterTrans.DailyReport.Web.Api.RequestObjects;
using WaterTrans.DailyReport.Web.Api.Resources;
using WaterTrans.DailyReport.Web.Api.ResponseObjects;
using WaterTrans.DailyReport.Web.Api.Security;

namespace WaterTrans.DailyReport.Web.Api.Controllers
{
    /// <summary>
    /// プロジェクトコントローラー
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Authorize(AuthenticationSchemes = BearerAuthenticationHandler.SchemeName)]
    [Authorize(Roles = Roles.Owner + "," + Roles.Contributor + "," + Roles.Reader)]
    public class ProjectsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IPersonService _personService;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mapper"><see cref="IMapper"/></param>
        /// <param name="projectService"><see cref="IProjectService"/></param>
        /// <param name="projectQueryService"><see cref="IProjectQueryService"/></param>
        /// <param name="personService"><see cref="IPersonService"/></param>
        public ProjectsController(
            IMapper mapper,
            IProjectService projectService,
            IProjectQueryService projectQueryService,
            IPersonService personService)
        {
            _mapper = mapper;
            _projectService = projectService;
            _projectQueryService = projectQueryService;
            _personService = personService;
        }

        /// <summary>
        /// プロジェクトを取得する
        /// </summary>
        /// <param name="projectId">プロジェクトID</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/projects/{projectId}")]
        [Authorize(Policies.ReadScopePolicy)]
        public ActionResult<Project> GetProject(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string projectId)
        {
            var projectGuid = Guid.Parse(projectId);
            var entity = _projectService.GetProject(projectGuid);

            if (entity == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            return _mapper.Map<Domain.Entities.Project, Project>(entity);
        }

        /// <summary>
        /// プロジェクトを検索する
        /// </summary>
        /// <param name="request"><see cref="ProjectQueryRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/projects")]
        [Authorize(Policies.ReadScopePolicy)]
        public ActionResult<PagedObject<Project>> QueryProject([FromQuery] ProjectQueryRequest request)
        {
            var dto = _mapper.Map<ProjectQueryRequest, ProjectQueryDto>(request);
            var entities = _projectService.QueryProject(dto);
            var result = new PagedObject<Project>
            {
                Page = dto.Page,
                PageSize = dto.PageSize,
                Total = dto.TotalCount,
                Items = _mapper.Map<IList<Domain.Entities.Project>, List<Project>>(entities),
            };
            return result;
        }

        /// <summary>
        /// プロジェクトを削除する
        /// </summary>
        /// <param name="projectId">プロジェクトID</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpDelete]
        [Route("api/v{version:apiVersion}/projects/{projectId}")]
        [Authorize(Policies.WriteScopePolicy)]
        public ActionResult DeleteProject(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string projectId)
        {
            var projectGuid = Guid.Parse(projectId);
            var entity = _projectService.GetProject(projectGuid);

            if (entity == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            if (entity.Persons.Count > 0)
            {
                return ErrorObjectResultFactory.ValidationErrorDetail(
                    string.Format(ErrorMessages.ValidationHasChildren, ErrorMessages.DisplayPerson),
                    "projectId");
            }

            _projectService.DeleteProject(projectGuid);
            return new OkResult();
        }

        /// <summary>
        /// プロジェクトを更新する
        /// </summary>
        /// <param name="projectId">プロジェクトID</param>
        /// <param name="request"><see cref="ProjectUpdateRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpPatch]
        [Route("api/v{version:apiVersion}/projects/{projectId}")]
        [Authorize(Policies.WriteScopePolicy)]
        public ActionResult<Project> UpdateProject(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string projectId,
            [FromBody] ProjectUpdateRequest request)
        {
            var projectGuid = Guid.Parse(projectId);
            var project = _projectService.GetProject(projectGuid);
            if (project == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            if (request.ProjectCode != null &&
                request.ProjectCode != project.ProjectCode &&
                _projectQueryService.ExistsProjectCode(request.ProjectCode))
            {
                return ErrorObjectResultFactory.ValidationErrorDetail(
                    string.Format(ErrorMessages.ValidationDuplicated, ErrorMessages.DisplayProjectProjectCode),
                    "projectCode");
            }

            var dto = _mapper.Map<ProjectUpdateRequest, ProjectUpdateDto>(request);
            dto.ProjectId = projectGuid;
            var entity = _projectService.UpdateProject(dto);
            return _mapper.Map<Domain.Entities.Project, Project>(entity);
        }

        /// <summary>
        /// プロジェクトを登録する
        /// </summary>
        /// <param name="request"><see cref="ProjectCreateRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/projects")]
        [Authorize(Policies.WriteScopePolicy)]
        public ActionResult<Project> CreateProject([FromBody] ProjectCreateRequest request)
        {
            if (_projectQueryService.ExistsProjectCode(request.ProjectCode))
            {
                return ErrorObjectResultFactory.ValidationErrorDetail(
                    string.Format(ErrorMessages.ValidationDuplicated, ErrorMessages.DisplayProjectProjectCode),
                    "projectCode");
            }

            var dto = _mapper.Map<ProjectCreateRequest, ProjectCreateDto>(request);
            var entity = _projectService.CreateProject(dto);
            return _mapper.Map<Domain.Entities.Project, Project>(entity);
        }

        /// <summary>
        /// プロジェクトに従業員を配属する
        /// </summary>
        /// <param name="projectId">プロジェクトID</param>
        /// <param name="personId">従業員ID</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/projects/{projectId}/persons/{personId}")]
        [Authorize(Policies.WriteScopePolicy)]
        public ActionResult<PagedObject<Project>> AddProjectPerson(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string projectId,
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string personId)
        {
            var projectGuid = Guid.Parse(projectId);
            var project = _projectService.GetProject(projectGuid);
            if (project == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            var personGuid = Guid.Parse(personId);
            var person = _personService.GetPerson(personGuid);
            if (person == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            _projectService.AddProjectPerson(projectGuid, personGuid);

            return new OkResult();
        }

        /// <summary>
        /// プロジェクトの従業員を解除する
        /// </summary>
        /// <param name="projectId">プロジェクトID</param>
        /// <param name="personId">従業員ID</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpDelete]
        [Route("api/v{version:apiVersion}/projects/{projectId}/persons/{personId}")]
        [Authorize(Policies.WriteScopePolicy)]
        public ActionResult<PagedObject<Project>> RemoveProjectPerson(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string projectId,
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string personId)
        {
            var projectGuid = Guid.Parse(projectId);
            var personGuid = Guid.Parse(personId);
            if (!_projectService.ContainsProjectPerson(projectGuid, personGuid))
            {
                return ErrorObjectResultFactory.NotFound();
            }

            _projectService.RemoveProjectPerson(projectGuid, personGuid);

            return new OkResult();
        }

        /// <summary>
        /// プロジェクトの従業員を検索する
        /// </summary>
        /// <param name="projectId">プロジェクトID</param>
        /// <param name="request"><see cref="ProjectPersonQueryRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/projects/{projectId}/persons")]
        [Authorize(Policies.ReadScopePolicy)]
        public ActionResult<PagedObject<Person>> QueryPerson(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string projectId,
            [FromQuery] ProjectPersonQueryRequest request)
        {
            var dto = _mapper.Map<ProjectPersonQueryRequest, ProjectPersonQueryDto>(request);
            dto.ProjectId = Guid.Parse(projectId);
            var entities = _projectService.QueryPerson(dto);
            var result = new PagedObject<Person>
            {
                Page = dto.Page,
                PageSize = dto.PageSize,
                Total = dto.TotalCount,
                Items = _mapper.Map<IList<Domain.Entities.Person>, List<Person>>(entities),
            };
            return result;
        }
    }
}
