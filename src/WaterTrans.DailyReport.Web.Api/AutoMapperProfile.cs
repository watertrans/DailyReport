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
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(e => e.Value).ToList()))
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreateTime.ToISO8601()))
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => src.UpdateTime.ToISO8601()));
            CreateMap<PersonUpdateRequest, PersonUpdateDto>();
            CreateMap<PersonQueryRequest, PersonQueryDto>()
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page ?? PagingQuery.DefaultPage))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? PagingQuery.DefaultPageSize))
                .ForMember(dest => dest.Sort, opt => opt.MapFrom(src => SortOrder.Parse(src.Sort)));

            // 部門関連
            CreateMap<GroupCreateRequest, GroupCreateDto>();
            CreateMap<Domain.Entities.Group, Group>()
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId.ToString()))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(e => e.Value).ToList()))
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreateTime.ToISO8601()))
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => src.UpdateTime.ToISO8601()));
            CreateMap<GroupUpdateRequest, GroupUpdateDto>();
            CreateMap<GroupQueryRequest, GroupQueryDto>()
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page ?? PagingQuery.DefaultPage))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize ?? PagingQuery.DefaultPageSize))
                .ForMember(dest => dest.Sort, opt => opt.MapFrom(src => SortOrder.Parse(src.Sort)));
        }
    }
}
