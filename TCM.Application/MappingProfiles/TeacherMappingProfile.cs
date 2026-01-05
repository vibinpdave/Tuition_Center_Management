using TCM.Application.Features.Teachers.Commands.Create;
using TCM.Application.Features.Teachers.Commands.Update;
using TCM.Domain.Enum;

namespace TCM.Application.MappingProfiles
{
    public class TeacherMappingProfile : Profile
    {
        public TeacherMappingProfile()
        {
            CreateMap<CreateTeacherCommand, Teacher>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.TeacherGradeSubjects, opt => opt.Ignore());

            CreateMap<UpdateTeacherCommand, Teacher>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.TeacherGradeSubjects, opt => opt.Ignore());

            CreateMap<Teacher, TeacherDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null))
                .ForMember(dest => dest.GradeSubjects, opt => opt.MapFrom(src => src.TeacherGradeSubjects));

            CreateMap<TeacherGradeSubjects, TeacherGradeSubjectDto>()
                .ForMember(dest => dest.GradeSubjectId, opt => opt.MapFrom(src => src.GradeSubjectId))
                .ForMember(dest => dest.GradeName, opt => opt.MapFrom(src => src.GradeSubjects.Grade.Name))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.GradeSubjects.Subject.Name));

            CreateMap<Teacher, TeacherListDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Status == (int)Enums.Status.Active));
        }
    }
}
