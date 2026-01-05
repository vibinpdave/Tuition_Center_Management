namespace TCM.Application.MappingProfiles
{
    public class TokenProfile : Profile
    {
        public TokenProfile()
        {
            CreateMap<(User user, (string AccessToken, string RefreshToken) tokens), AccessTokenResponseDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.user.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.user.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.user.Email))
            .ForMember(dest => dest.UserRoleId, opt => opt.MapFrom(src => src.user.UserRoleId))
            .ForMember(dest => dest.TokenDetails, opt => opt.MapFrom(src => src)); // Map nested DTO


            CreateMap<(User user, (string AccessToken, string RefreshToken) tokens), TokenDetailsDTO>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.tokens.AccessToken))
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.tokens.RefreshToken))
                .ForMember(dest => dest.AccessTokenExpiresAt, o => o.Ignore())
                .ForMember(dest => dest.RefreshTokenExpiresAt, o => o.Ignore());

            CreateMap<(string AccessToken, string RefreshToken), TokenDetailsDTO>()
             .ForMember(d => d.AccessToken, o => o.MapFrom(s => s.AccessToken))
             .ForMember(d => d.RefreshToken, o => o.MapFrom(s => s.RefreshToken))
             .ForMember(d => d.AccessTokenExpiresAt, o => o.Ignore())
             .ForMember(d => d.RefreshTokenExpiresAt, o => o.Ignore());
        }
    }
}
