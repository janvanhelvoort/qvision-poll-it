namespace Qvision.Umbraco.PollIt.Mapping.Profile
{
    using AutoMapper;

    public class AnswerProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Models.Pocos.Answer, Models.Answer>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(x => x.Index, opt => opt.MapFrom(src => src.Index))
                .ForMember(x => x.Percentage, opt => opt.Ignore());
        }
    }
}