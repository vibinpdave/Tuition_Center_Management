using TCM.Application.DTOs;
using TCM.Application.Features.Grades.Commands.CreateGrade;

namespace TCM.Application.MappingProfiles
{
    public class GradeProfile : Profile
    {
        public GradeProfile()
        {
            CreateMap<CreateGradeCommand, Grade>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Grade, GradeDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)).ReverseMap();
        }
    }
}
