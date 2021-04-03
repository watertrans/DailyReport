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
    /// 従業員コントローラー
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Authorize(AuthenticationSchemes = BearerAuthenticationHandler.SchemeName)]
    [Authorize(Roles = Roles.Owner + "," + Roles.Contributor + "," + Roles.Reader)]
    public class PersonsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPersonService _personService;
        private readonly IPersonQueryService _personQueryService;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mapper"><see cref="IMapper"/></param>
        /// <param name="personService"><see cref="IPersonService"/></param>
        /// <param name="personQueryService"><see cref="IPersonQueryService"/></param>
        public PersonsController(
            IMapper mapper,
            IPersonService personService,
            IPersonQueryService personQueryService)
        {
            _mapper = mapper;
            _personService = personService;
            _personQueryService = personQueryService;
        }

        /// <summary>
        /// 従業員を取得する
        /// </summary>
        /// <param name="personId">従業員ID</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/persons/{personId}")]
        [Authorize(Policies.ReadScopePolicy)]
        public ActionResult<Person> GetPerson(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string personId)
        {
            var personGuid = Guid.Parse(personId);
            var entity = _personService.GetPerson(personGuid);

            if (entity == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            return _mapper.Map<Domain.Entities.Person, Person>(entity);
        }

        /// <summary>
        /// 従業員を検索する
        /// </summary>
        /// <param name="request"><see cref="PersonQueryRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/persons")]
        [Authorize(Policies.ReadScopePolicy)]
        public ActionResult<PagedObject<Person>> QueryPerson([FromQuery] PersonQueryRequest request)
        {
            var dto = _mapper.Map<PersonQueryRequest, PersonQueryDto>(request);
            var entities = _personService.QueryPerson(dto);
            var result = new PagedObject<Person>
            {
                Page = dto.Page,
                PageSize = dto.PageSize,
                Total = dto.TotalCount,
                Items = _mapper.Map<IList<Domain.Entities.Person>, List<Person>>(entities),
            };
            return result;
        }

        /// <summary>
        /// 従業員を削除する
        /// </summary>
        /// <param name="personId">従業員ID</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpDelete]
        [Route("api/v{version:apiVersion}/persons/{personId}")]
        [Authorize(Policies.WriteScopePolicy)]
        public ActionResult DeletePerson(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string personId)
        {
            var personGuid = Guid.Parse(personId);
            var entity = _personService.GetPerson(personGuid);

            if (entity == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            _personService.DeletePerson(personGuid);
            return new OkResult();
        }

        /// <summary>
        /// 従業員を更新する
        /// </summary>
        /// <param name="personId">従業員ID</param>
        /// <param name="request"><see cref="PersonUpdateRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpPatch]
        [Route("api/v{version:apiVersion}/persons/{personId}")]
        [Authorize(Policies.WriteScopePolicy)]
        public ActionResult<Person> UpdatePerson(
            [FromRoute]
            [Required(ErrorMessage = "DataAnnotationRequired")]
            [Guid(ErrorMessage = "DataAnnotationGuid")]
            string personId,
            [FromBody] PersonUpdateRequest request)
        {
            var personGuid = Guid.Parse(personId);
            var person = _personService.GetPerson(personGuid);
            if (person == null)
            {
                return ErrorObjectResultFactory.NotFound();
            }

            if (request.PersonCode != null &&
                request.PersonCode != person.PersonCode &&
                _personQueryService.ExistsPersonCode(request.PersonCode))
            {
                return ErrorObjectResultFactory.ValidationErrorDetail(
                    string.Format(ErrorMessages.ValidationDuplicated, ErrorMessages.DisplayPersonPersonCode),
                    "personCode");
            }

            var dto = _mapper.Map<PersonUpdateRequest, PersonUpdateDto>(request);
            dto.PersonId = personGuid;
            var entity = _personService.UpdatePerson(dto);
            return _mapper.Map<Domain.Entities.Person, Person>(entity);
        }

        /// <summary>
        /// 従業員を登録する
        /// </summary>
        /// <param name="request"><see cref="PersonCreateRequest"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/persons")]
        [Authorize(Policies.WriteScopePolicy)]
        public ActionResult<Person> CreatePerson([FromBody] PersonCreateRequest request)
        {
            if (_personQueryService.ExistsPersonCode(request.PersonCode))
            {
                return ErrorObjectResultFactory.ValidationErrorDetail(
                    string.Format(ErrorMessages.ValidationDuplicated, ErrorMessages.DisplayPersonPersonCode),
                    "personCode");
            }

            var dto = _mapper.Map<PersonCreateRequest, PersonCreateDto>(request);
            var entity = _personService.CreatePerson(dto);
            return _mapper.Map<Domain.Entities.Person, Person>(entity);
        }
    }
}
