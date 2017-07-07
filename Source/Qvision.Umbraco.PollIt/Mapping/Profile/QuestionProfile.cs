namespace Qvision.Umbraco.PollIt.Mapping.Profile
{
    using AutoMapper;

    public class QuestionProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Models.Pocos.Question, Models.Question>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Responses, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(x => x.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(x => x.EndDate, opt => opt.MapFrom(src => src.EndDate));
        }
    }
}
