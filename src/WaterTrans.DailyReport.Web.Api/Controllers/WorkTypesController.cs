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
    /// 業務分類コントローラー
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Authorize(AuthenticationSchemes = BearerAuthenticationHandler.SchemeName)]
    public class WorkTypesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IWorkTypeService _workTypeService;
        private readonly IWorkTypeQueryService _workTypeQueryService;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mapper"><see cref="IMapper"/></param>
        /// <param name="workTypeService"><see cref="IWorkTypeService"/></param>
        /// <param name="workTypeQueryService"><see cref="IWorkTypeQueryService"/></param>
        public WorkTypesController(
            IMapper mapper,
            IWorkTypeService workTypeService,
            IWorkTypeQueryService workTypeQueryService)
        {
            _mapper = mapper;
            _workTypeService = workTypeService;
            _workTypeQueryService = workTypeQueryService;
        }

        /// <summary>
        /// 業務分類を取得する
        /// </summary>
        /// <param name="workTypeId">業務分類ID</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/workTypes/{workTypeId}")]
        [Authorize(Policies.ReadScopePolicy)]
        [Authorize(Roles = Roles.Owner + "," + Roles.Contributor + "," + Roles.Reader + "," + Roles.User)]
        public ActionResult<WorkType> GetWorkType(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string workTypeId)
        {
            var workTypeGuid = Guid.Parse(workTypeId);
            var entity = _workTypeService.GetWorkType(workTypeGuid);

            if (entity == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            return _mapper.Map<Domain.Entities.WorkType, WorkType>(entity);
        }

        /// <summary>
        /// 業務分類を検索する
        /// </summary>
        /// <param name="request"><see cref="WorkTypeQueryRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/workTypes")]
        [Authorize(Policies.ReadScopePolicy)]
        [Authorize(Roles = Roles.Owner + "," + Roles.Contributor + "," + Roles.Reader + "," + Roles.User)]
        public ActionResult<PagedObject<WorkType>> QueryWorkType([FromQuery] WorkTypeQueryRequest request)
        {
            var dto = _mapper.Map<WorkTypeQueryRequest, WorkTypeQueryDto>(request);
            var entities = _workTypeService.QueryWorkType(dto);
            var result = new PagedObject<WorkType>
            {
                Page = dto.Page,
                PageSize = dto.PageSize,
                Total = dto.TotalCount,
                Items = _mapper.Map<IList<Domain.Entities.WorkType>, List<WorkType>>(entities),
            };
            return result;
        }

        /// <summary>
        /// 業務分類を削除する
        /// </summary>
        /// <param name="workTypeId">業務分類ID</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpDelete]
        [Route("api/v{version:apiVersion}/workTypes/{workTypeId}")]
        [Authorize(Policies.WriteScopePolicy)]
        [Authorize(Roles = Roles.Owner + "," + Roles.Contributor)]
        public ActionResult DeleteWorkType(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string workTypeId)
        {
            var workTypeGuid = Guid.Parse(workTypeId);
            var entity = _workTypeService.GetWorkType(workTypeGuid);

            if (entity == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            _workTypeService.DeleteWorkType(workTypeGuid);
            return new OkResult();
        }

        /// <summary>
        /// 業務分類を更新する
        /// </summary>
        /// <param name="workTypeId">業務分類ID</param>
        /// <param name="request"><see cref="WorkTypeUpdateRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpPatch]
        [Route("api/v{version:apiVersion}/workTypes/{workTypeId}")]
        [Authorize(Policies.WriteScopePolicy)]
        [Authorize(Roles = Roles.Owner + "," + Roles.Contributor)]
        public ActionResult<WorkType> UpdateWorkType(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string workTypeId,
            [FromBody] WorkTypeUpdateRequest request)
        {
            var workTypeGuid = Guid.Parse(workTypeId);
            var workType = _workTypeService.GetWorkType(workTypeGuid);
            if (workType == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            if (request.WorkTypeCode != null &&
                request.WorkTypeCode != workType.WorkTypeCode &&
                _workTypeQueryService.ExistsWorkTypeCode(request.WorkTypeCode))
            {
                return ErrorObjectResultFactory.ValidationErrorDetail(
                    string.Format(ErrorMessages.ValidationDuplicated, ErrorMessages.DisplayWorkTypeWorkTypeCode),
                    "workTypeCode");
            }

            if (request.WorkTypeTree != null &&
                request.WorkTypeTree != workType.WorkTypeTree &&
                _workTypeQueryService.ExistsWorkTypeTree(request.WorkTypeTree))
            {
                return ErrorObjectResultFactory.ValidationErrorDetail(
                    string.Format(ErrorMessages.ValidationDuplicated, ErrorMessages.DisplayWorkTypeWorkTypeTree),
                    "workTypeTree");
            }

            var dto = _mapper.Map<WorkTypeUpdateRequest, WorkTypeUpdateDto>(request);
            dto.WorkTypeId = workTypeGuid;
            var entity = _workTypeService.UpdateWorkType(dto);
            return _mapper.Map<Domain.Entities.WorkType, WorkType>(entity);
        }

        /// <summary>
        /// 業務分類を登録する
        /// </summary>
        /// <param name="request"><see cref="WorkTypeCreateRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/workTypes")]
        [Authorize(Policies.WriteScopePolicy)]
        [Authorize(Roles = Roles.Owner + "," + Roles.Contributor)]
        public ActionResult<WorkType> CreateWorkType([FromBody] WorkTypeCreateRequest request)
        {
            if (_workTypeQueryService.ExistsWorkTypeCode(request.WorkTypeCode))
            {
                return ErrorObjectResultFactory.ValidationErrorDetail(
                    string.Format(ErrorMessages.ValidationDuplicated, ErrorMessages.DisplayWorkTypeWorkTypeCode),
                    "workTypeCode");
            }

            if (_workTypeQueryService.ExistsWorkTypeTree(request.WorkTypeTree))
            {
                return ErrorObjectResultFactory.ValidationErrorDetail(
                    string.Format(ErrorMessages.ValidationDuplicated, ErrorMessages.DisplayWorkTypeWorkTypeTree),
                    "workTypeTree");
            }

            var dto = _mapper.Map<WorkTypeCreateRequest, WorkTypeCreateDto>(request);
            var entity = _workTypeService.CreateWorkType(dto);
            return _mapper.Map<Domain.Entities.WorkType, WorkType>(entity);
        }
    }
}
