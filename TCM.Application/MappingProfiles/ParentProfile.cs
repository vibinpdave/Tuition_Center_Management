using TCM.Application.Features.Parents.Commands.Create;
using TCM.Application.Features.Parents.Commands.Update;
using static TCM.Domain.Enum.Enums;

namespace TCM.Application.MappingProfiles
{
    public class ParentProfile : Profile
    {
        public ParentProfile()
        {
            CreateMap<CreateParentCommand, Parent>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => true));

            CreateMap<CreateParentCommand, User>()
                .ForMember(d => d.Password, o => o.Ignore())
                .ForMember(d => d.UserRoleId, o => o.Ignore());

            CreateMap<UpdateParentCommand, Parent>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<Parent, ParentDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Status == (int)Status.Active));

            CreateMap<Parent, ParentListDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Status == (int)Status.Active));
        }
    }
}
