using TCM.Application.Features.Students.Commands.Create;

namespace TCM.Application.MappingProfiles
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            CreateMap<CreateStudentCommand, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password)) // we will hash separately
                .ForMember(dest => dest.UserRoleId, opt => opt.Ignore()); // set manually

            CreateMap<CreateStudentCommand, Student>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.MobileNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ResidentialAddress, opt => opt.MapFrom(src => src.ResidentialAddress))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dest => dest.GradeId, opt => opt.MapFrom(src => src.GradeId))
                .ForMember(dest => dest.UserId, opt => opt.Ignore()); // will set after user creation
        }
    }
}
