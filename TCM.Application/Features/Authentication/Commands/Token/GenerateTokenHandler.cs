using TCM.Application.Contracts.Infrastructure;

namespace TCM.Application.Features.Authentication.Commands.Token
{
    public record GenerateTokenCommand(long UserId) : IRequest<GenerateTokenResult>;
    public record GenerateTokenResult(AccessTokenResponseDTO UserDetails);
    public class GenerateTokenValidator : AbstractValidator<GenerateTokenCommand>
    {
        public GenerateTokenValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is missing");
        }
    }
    internal class GenerateTokenHandler : IRequestHandler<GenerateTokenCommand, GenerateTokenResult>
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public GenerateTokenHandler(ITokenService tokenService, IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<GenerateTokenResult> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
        {
            var userDetails = await _unitOfWork.Users.GetByIdAsync(request.UserId);

            var tokens = _tokenService.GenerateTokens(userDetails.Name, userDetails.Email, request.UserId, userDetails.UserRoleId);

            // Map to DTO
            var tokenResponse = _mapper.Map<AccessTokenResponseDTO>((userDetails, tokens));
            if (tokenResponse != null)
            {
                tokenResponse.TokenDetails.AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpirationMinutes"]));
                tokenResponse.TokenDetails.RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpirationMinutes"]));
            }

            var result = new GenerateTokenResult(tokenResponse);

            return result;
        }
    }
}
