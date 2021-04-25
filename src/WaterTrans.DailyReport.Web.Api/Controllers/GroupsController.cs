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
    /// 部署コントローラー
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Authorize(AuthenticationSchemes = BearerAuthenticationHandler.SchemeName)]
    public class GroupsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;
        private readonly IGroupQueryService _groupQueryService;
        private readonly IPersonService _personService;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mapper"><see cref="IMapper"/></param>
        /// <param name="groupService"><see cref="IGroupService"/></param>
        /// <param name="groupQueryService"><see cref="IGroupQueryService"/></param>
        /// <param name="personService"><see cref="IPersonService"/></param>
        public GroupsController(
            IMapper mapper,
            IGroupService groupService,
            IGroupQueryService groupQueryService,
            IPersonService personService)
        {
            _mapper = mapper;
            _groupService = groupService;
            _groupQueryService = groupQueryService;
            _personService = personService;
        }

        /// <summary>
        /// 部署を取得する
        /// </summary>
        /// <param name="groupId">部署ID</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/groups/{groupId}")]
        [Authorize(Policies.ReadScopePolicy)]
        [Authorize(Roles = Roles.Owner + "," + Roles.Contributor + "," + Roles.Reader + "," + Roles.User)]
        public ActionResult<Group> GetGroup(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string groupId)
        {
            var groupGuid = Guid.Parse(groupId);
            var entity = _groupService.GetGroup(groupGuid);

            if (entity == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            return _mapper.Map<Domain.Entities.Group, Group>(entity);
        }

        /// <summary>
        /// 部署を検索する
        /// </summary>
        /// <param name="request"><see cref="GroupQueryRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/groups")]
        [Authorize(Policies.ReadScopePolicy)]
        [Authorize(Roles = Roles.Owner + "," + Roles.Contributor + "," + Roles.Reader + "," + Roles.User)]
        public ActionResult<PagedObject<Group>> QueryGroup([FromQuery] GroupQueryRequest request)
        {
            var dto = _mapper.Map<GroupQueryRequest, GroupQueryDto>(request);
            var entities = _groupService.QueryGroup(dto);
            var result = new PagedObject<Group>
            {
                Page = dto.Page,
                PageSize = dto.PageSize,
                Total = dto.TotalCount,
                Items = _mapper.Map<IList<Domain.Entities.Group>, List<Group>>(entities),
            };
            return result;
        }

        /// <summary>
        /// 部署を削除する
        /// </summary>
        /// <param name="groupId">部署ID</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpDelete]
        [Route("api/v{version:apiVersion}/groups/{groupId}")]
        [Authorize(Policies.WriteScopePolicy)]
        [Authorize(Roles = Roles.Owner + "," + Roles.Contributor)]
        public ActionResult DeleteGroup(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string groupId)
        {
            var groupGuid = Guid.Parse(groupId);
            var entity = _groupService.GetGroup(groupGuid);

            if (entity == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            if (entity.Persons.Count > 0)
            {
                return ErrorObjectResultFactory.ValidationErrorDetail(
                    string.Format(ErrorMessages.ValidationHasChildren, ErrorMessages.DisplayPerson),
                    "groupId");
            }

            _groupService.DeleteGroup(groupGuid);
            return new OkResult();
        }

        /// <summary>
        /// 部署を更新する
        /// </summary>
        /// <param name="groupId">部署ID</param>
        /// <param name="request"><see cref="GroupUpdateRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpPatch]
        [Route("api/v{version:apiVersion}/groups/{groupId}")]
        [Authorize(Policies.WriteScopePolicy)]
        [Authorize(Roles = Roles.Owner + "," + Roles.Contributor)]
        public ActionResult<Group> UpdateGroup(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string groupId,
            [FromBody] GroupUpdateRequest request)
        {
            var groupGuid = Guid.Parse(groupId);
            var group = _groupService.GetGroup(groupGuid);
            if (group == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            if (request.GroupCode != null &&
                request.GroupCode != group.GroupCode &&
                _groupQueryService.ExistsGroupCode(request.GroupCode))
            {
                return ErrorObjectResultFactory.ValidationErrorDetail(
                    string.Format(ErrorMessages.ValidationDuplicated, ErrorMessages.DisplayGroupGroupCode),
                    "groupCode");
            }

            if (request.GroupTree != null &&
                request.GroupTree != group.GroupTree &&
                _groupQueryService.ExistsGroupTree(request.GroupTree))
            {
                return ErrorObjectResultFactory.ValidationErrorDetail(
                    string.Format(ErrorMessages.ValidationDuplicated, ErrorMessages.DisplayGroupGroupTree),
                    "groupTree");
            }

            var dto = _mapper.Map<GroupUpdateRequest, GroupUpdateDto>(request);
            dto.GroupId = groupGuid;
            var entity = _groupService.UpdateGroup(dto);
            return _mapper.Map<Domain.Entities.Group, Group>(entity);
        }

        /// <summary>
        /// 部署を登録する
        /// </summary>
        /// <param name="request"><see cref="GroupCreateRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/groups")]
        [Authorize(Policies.WriteScopePolicy)]
        [Authorize(Roles = Roles.Owner + "," + Roles.Contributor)]
        public ActionResult<Group> CreateGroup([FromBody] GroupCreateRequest request)
        {
            if (_groupQueryService.ExistsGroupCode(request.GroupCode))
            {
                return ErrorObjectResultFactory.ValidationErrorDetail(
                    string.Format(ErrorMessages.ValidationDuplicated, ErrorMessages.DisplayGroupGroupCode),
                    "groupCode");
            }

            if (_groupQueryService.ExistsGroupTree(request.GroupTree))
            {
                return ErrorObjectResultFactory.ValidationErrorDetail(
                    string.Format(ErrorMessages.ValidationDuplicated, ErrorMessages.DisplayGroupGroupTree),
                    "groupTree");
            }

            var dto = _mapper.Map<GroupCreateRequest, GroupCreateDto>(request);
            var entity = _groupService.CreateGroup(dto);
            return _mapper.Map<Domain.Entities.Group, Group>(entity);
        }

        /// <summary>
        /// 部署に従業員を配属する
        /// </summary>
        /// <param name="groupId">部署ID</param>
        /// <param name="personId">従業員ID</param>
        /// <param name="request"><see cref="GroupPersonAddRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/groups/{groupId}/persons/{personId}")]
        [Authorize(Policies.WriteScopePolicy)]
        [Authorize(Roles = Roles.Owner + "," + Roles.Contributor)]
        public ActionResult<PagedObject<Group>> AddGroupPerson(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string groupId,
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string personId,
            [FromBody]
            GroupPersonAddRequest request)
        {
            var groupGuid = Guid.Parse(groupId);
            var group = _groupService.GetGroup(groupGuid);
            if (group == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            var personGuid = Guid.Parse(personId);
            var person = _personService.GetPerson(personGuid);
            if (person == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            _groupService.AddGroupPerson(groupGuid, personGuid, request.PositionType);

            return new OkResult();
        }

        /// <summary>
        /// 部署の従業員を解除する
        /// </summary>
        /// <param name="groupId">部署ID</param>
        /// <param name="personId">従業員ID</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpDelete]
        [Route("api/v{version:apiVersion}/groups/{groupId}/persons/{personId}")]
        [Authorize(Policies.WriteScopePolicy)]
        [Authorize(Roles = Roles.Owner + "," + Roles.Contributor)]
        public ActionResult<PagedObject<Group>> RemoveGroupPerson(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string groupId,
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string personId)
        {
            var groupGuid = Guid.Parse(groupId);
            var personGuid = Guid.Parse(personId);
            if (!_groupService.ContainsGroupPerson(groupGuid, personGuid))
            {
                return ErrorObjectResultFactory.NotFound();
            }

            _groupService.RemoveGroupPerson(groupGuid, personGuid);

            return new OkResult();
        }

        /// <summary>
        /// 部署の従業員を検索する
        /// </summary>
        /// <param name="groupId">部署ID</param>
        /// <param name="request"><see cref="GroupPersonQueryRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/groups/{groupId}/persons")]
        [Authorize(Policies.ReadScopePolicy)]
        [Authorize(Roles = Roles.Owner + "," + Roles.Contributor + "," + Roles.Reader + "," + Roles.User)]
        public ActionResult<PagedObject<GroupPerson>> QueryPerson(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string groupId,
            [FromQuery] GroupPersonQueryRequest request)
        {
            var dto = _mapper.Map<GroupPersonQueryRequest, GroupPersonQueryDto>(request);
            dto.GroupId = Guid.Parse(groupId);
            var entities = _groupService.QueryPerson(dto);
            var result = new PagedObject<GroupPerson>
            {
                Page = dto.Page,
                PageSize = dto.PageSize,
                Total = dto.TotalCount,
                Items = _mapper.Map<IList<Domain.Entities.GroupPerson>, List<GroupPerson>>(entities),
            };
            return result;
        }
    }
}
