using TCM.Application.Contracts.Infrastructure;

namespace TCM.Application.Features.Authentication.Commands.Token
{
    public record GenerateTokenByRefreshTokenCommand(string RefreshToken) : IRequest<GenerateTokenByRefreshTokenResult>;
    public record GenerateTokenByRefreshTokenResult(TokenDetailsDTO TokenDetails);

    public class GenerateTokenByRefreshTokenValidator : AbstractValidator<GenerateTokenByRefreshTokenCommand>
    {
        public GenerateTokenByRefreshTokenValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("RefreshToken is missing");
        }
    }

    internal class GenerateTokenByRefreshTokenHandler : IRequestHandler<GenerateTokenByRefreshTokenCommand, GenerateTokenByRefreshTokenResult>
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public GenerateTokenByRefreshTokenHandler(ITokenService tokenService, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        public async Task<GenerateTokenByRefreshTokenResult> Handle(GenerateTokenByRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokens = _tokenService.RefreshTokens(request.RefreshToken);

            var tokenResponse = _mapper.Map<TokenDetailsDTO>(tokens);
            if (tokenResponse != null)
            {
                tokenResponse.AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpirationMinutes"]));
                tokenResponse.RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpirationMinutes"]));
            }
            var result = new GenerateTokenByRefreshTokenResult(tokenResponse);

            return result;
        }
    }
}
