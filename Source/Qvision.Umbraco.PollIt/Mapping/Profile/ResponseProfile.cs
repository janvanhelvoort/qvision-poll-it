namespace Qvision.PollIt.Mapping.Profile
{
    using AutoMapper;

    public class ResponseProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Models.Pocos.Response, Models.Response>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.ResponseDate, opt => opt.MapFrom(src => src.ResponseDate));
        }
    }
}