using TCM.Application.Contracts.Infrastructure;

namespace TCM.Application.Features.Authentication.Commands.Login
{
    public record UserLoginCommand(string UserName, string Password) : IRequest<UserLoginResult>;

    public record UserLoginResult(UserLoginResponseDTO objResponse);

    public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
    {
        public UserLoginCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }

    internal class UserLoginHandler : IRequestHandler<UserLoginCommand, UserLoginResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public readonly IPasswordHasher _passwordHasher;
        public UserLoginHandler(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }
        public async Task<UserLoginResult> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(request.UserName);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            // Password verification happens here (NOT in repo)
            if (!_passwordHasher.VerifyPassword(request.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            // Map User → DTO
            var userDto = _mapper.Map<UserLoginResponseDTO>(user);

            // Wrap DTO in Result
            var result = new UserLoginResult(userDto);

            return result;
        }
    }
}
