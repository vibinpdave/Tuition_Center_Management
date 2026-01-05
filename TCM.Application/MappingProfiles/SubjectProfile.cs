using TCM.Application.DTOs;
using TCM.Application.Features.Subjects.Commands.CreateSubject;

namespace TCM.Application.MappingProfiles
{
    public class SubjectProfile : Profile
    {
        public SubjectProfile()
        {
            CreateMap<CreateSubjectCommand, Subject>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Subject, SubjectDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid)).ReverseMap();

        }
    }
}
