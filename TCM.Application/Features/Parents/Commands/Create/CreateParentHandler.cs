namespace TCM.Application.Features.Parents.Commands.Create
{
    public record CreateParentCommand(
        string Name,
        string MobileNumber,
        string Email,
        string Password,
        string ResidentialAddress,
        string City,
        string State,
        string Country) : IRequest<CreateParentResult>;
    public record CreateParentResult(long Id);
    public class CreateParentCommandValidator : AbstractValidator<CreateParentCommand>
    {
        public CreateParentCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MustAsync(async (email, ct) =>
                {
                    return !unitOfWork.GetRepository<User>()
                        .GetAll(u => u.Email == email)
                        .Any();
                })
                .WithMessage(ValidationMessages.EMAIL_ALREADY_EXISTS);

            RuleFor(x => x.MobileNumber)
                .NotEmpty()
                .Matches(RegexPatterns.MOBILE_NUMBER)
                .WithMessage(ValidationMessages.INVALID_MOBILE);

            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(RegexPatterns.PASSWORD)
                .WithMessage(ValidationMessages.WEAK_PASSWORD);

            RuleFor(x => x.ResidentialAddress)
                .MaximumLength(250);

            RuleFor(x => x.City)
                .MaximumLength(50);

            RuleFor(x => x.State)
                .MaximumLength(50);

            RuleFor(x => x.Country)
                .MaximumLength(50);
        }
    }
    internal class CreateParentHandler : IRequestHandler<CreateParentCommand, CreateParentResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        public CreateParentHandler(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }
        public async Task<CreateParentResult> Handle(CreateParentCommand request, CancellationToken cancellationToken)
        {
            var userRepo = _unitOfWork.GetRepository<User>();

            // Get Parent Role
            var parentRole = _unitOfWork.GetRepository<UserRoles>()
                .Single(x => x.Name == "Parent");

            if (parentRole == null)
                throw new ArgumentException(ValidationMessages.ROLE_NOT_FOUND);

            var user = _mapper.Map<User>(request);
            user.Password = _passwordHasher.HashPassword(request.Password);
            user.UserRoleId = parentRole.Id;

            userRepo.Insert(user);
            _unitOfWork.SaveChanges(); // Needed to generate UserId

            //Create Teacher
            var parent = _mapper.Map<Parent>(request);
            parent.UserId = user.Id;

            _unitOfWork.GetRepository<Parent>().Insert(parent);
            _unitOfWork.SaveChanges();

            return new CreateParentResult(parent.Id);
        }
    }
}
