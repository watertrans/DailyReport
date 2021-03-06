using AutoMapper;
using System.Linq;
using WaterTrans.DailyReport.Application;
using WaterTrans.DailyReport.Application.DataTransferObjects;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Web.Api.RequestObjects;
using WaterTrans.DailyReport.Web.Api.ResponseObjects;

namespace WaterTrans.DailyReport.Web.Api
{
    /// <summary>
    /// AutoMapperProfile
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AutoMapperProfile()
        {
            AllowNullCollections = true;

            // 従業員関連
            CreateMap<PersonCreateRequest, PersonCreateDto>();
            CreateMap<Domain.Entities.Person, Person>()
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId.ToString()))
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreateTime.ToISO8601()))
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => src.UpdateTime.ToISO8601()));
            CreateMap<PersonUpdateRequest, PersonUpdateDto>();
            CreateMap<PersonQueryRequest, PersonQueryDto>()
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page ?? PagingQuery.DefaultPage))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? PagingQuery.DefaultPageSize))
                .ForMember(dest => dest.Sort, opt => opt.MapFrom(src => SortOrder.Parse(src.Sort)));

            // 部署関連
            CreateMap<GroupCreateRequest, GroupCreateDto>();
            CreateMap<Domain.Entities.Group, Group>()
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId.ToString()))
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreateTime.ToISO8601()))
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => src.UpdateTime.ToISO8601()));
            CreateMap<Domain.Entities.GroupPerson, GroupPerson>()
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId.ToString()))
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreateTime.ToISO8601()))
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => src.UpdateTime.ToISO8601()));
            CreateMap<GroupUpdateRequest, GroupUpdateDto>();
            CreateMap<GroupPersonQueryRequest, GroupPersonQueryDto>()
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page ?? PagingQuery.DefaultPage))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? PagingQuery.DefaultPageSize))
                .ForMember(dest => dest.Sort, opt => opt.MapFrom(src => SortOrder.Parse(src.Sort)));
            CreateMap<GroupQueryRequest, GroupQueryDto>()
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page ?? PagingQuery.DefaultPage))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? PagingQuery.DefaultPageSize))
                .ForMember(dest => dest.Sort, opt => opt.MapFrom(src => SortOrder.Parse(src.Sort)));

            // プロジェクト関連
            CreateMap<ProjectCreateRequest, ProjectCreateDto>();
            CreateMap<Domain.Entities.Project, Project>()
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId.ToString()))
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreateTime.ToISO8601()))
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => src.UpdateTime.ToISO8601()));
            CreateMap<ProjectUpdateRequest, ProjectUpdateDto>();
            CreateMap<ProjectPersonQueryRequest, ProjectPersonQueryDto>()
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page ?? PagingQuery.DefaultPage))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? PagingQuery.DefaultPageSize))
                .ForMember(dest => dest.Sort, opt => opt.MapFrom(src => SortOrder.Parse(src.Sort)));
            CreateMap<ProjectQueryRequest, ProjectQueryDto>()
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page ?? PagingQuery.DefaultPage))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? PagingQuery.DefaultPageSize))
                .ForMember(dest => dest.Sort, opt => opt.MapFrom(src => SortOrder.Parse(src.Sort)));

            // 業務分類関連
            CreateMap<WorkTypeCreateRequest, WorkTypeCreateDto>();
            CreateMap<Domain.Entities.WorkType, WorkType>()
                .ForMember(dest => dest.WorkTypeId, opt => opt.MapFrom(src => src.WorkTypeId.ToString()))
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreateTime.ToISO8601()))
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => src.UpdateTime.ToISO8601()));
            CreateMap<WorkTypeUpdateRequest, WorkTypeUpdateDto>();
            CreateMap<WorkTypeQueryRequest, WorkTypeQueryDto>()
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page ?? PagingQuery.DefaultPage))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? PagingQuery.DefaultPageSize))
                .ForMember(dest => dest.Sort, opt => opt.MapFrom(src => SortOrder.Parse(src.Sort)));
        }
    }
}
