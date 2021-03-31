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
    /// 部門コントローラー
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Authorize(AuthenticationSchemes = BearerAuthenticationHandler.SchemeName)]
    [Authorize(Roles = Roles.Owner + "," + Roles.Contributor + "," + Roles.Reader)]
    public class GroupsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;
        private readonly IGroupQueryService _groupQueryService;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mapper"><see cref="IMapper"/></param>
        /// <param name="groupService"><see cref="IGroupService"/></param>
        /// <param name="groupQueryService"><see cref="IGroupQueryService"/></param>
        public GroupsController(
            IMapper mapper,
            IGroupService groupService,
            IGroupQueryService groupQueryService)
        {
            _mapper = mapper;
            _groupService = groupService;
            _groupQueryService = groupQueryService;
        }

        /// <summary>
        /// 部門を取得する
        /// </summary>
        /// <param name="groupId">部門ID</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/groups/{groupId}")]
        [Authorize(Policies.ReadScopePolicy)]
        public ActionResult<Group> Get(
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
        /// 部門を検索する
        /// </summary>
        /// <param name="request"><see cref="GroupQueryRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/groups")]
        [Authorize(Policies.ReadScopePolicy)]
        public ActionResult<PagedObject<Group>> Get([FromQuery] GroupQueryRequest request)
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
        /// 部門を削除する
        /// </summary>
        /// <param name="groupId">部門ID</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpDelete]
        [Route("api/v{version:apiVersion}/groups/{groupId}")]
        [Authorize(Policies.WriteScopePolicy)]
        public ActionResult Delete(
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

            _groupService.DeleteGroup(groupGuid);
            return new OkResult();
        }

        /// <summary>
        /// 部門を更新する
        /// </summary>
        /// <param name="groupId">部門ID</param>
        /// <param name="request"><see cref="GroupUpdateRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpPatch]
        [Route("api/v{version:apiVersion}/groups/{groupId}")]
        [Authorize(Policies.WriteScopePolicy)]
        public ActionResult<Group> Patch(
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
        /// 部門を登録する
        /// </summary>
        /// <param name="request"><see cref="GroupCreateRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/groups")]
        [Authorize(Policies.WriteScopePolicy)]
        public ActionResult<Group> Post([FromBody] GroupCreateRequest request)
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
    }
}
