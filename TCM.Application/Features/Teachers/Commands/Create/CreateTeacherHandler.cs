namespace TCM.Application.Features.Teachers.Commands.Create
{
    public record CreateTeacherCommand(
        string Name,
        string Email,
        string MobileNumber,
        string Password,
        string ResidentialAddress,
        string City,
        string State,
        string Country,
        string Qualification) : IRequest<CreateTeacherResult>;

    public record CreateTeacherResult(long Id);

    #region CreateTeacherValidator
    public class CreateTeacherValidator : AbstractValidator<CreateTeacherCommand>
    {
        public CreateTeacherValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(
                    string.Format(ValidationMessages.NAME_REQUIRED, nameof(Teacher)));

            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress()
                .WithMessage(ValidationMessages.EMAIL_REQUIRED);

            RuleFor(x => x.MobileNumber)
                .NotEmpty()
                .WithMessage(ValidationMessages.MOBILE_REQUIRED);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
    #endregion
    internal class CreateTeacherHandler : IRequestHandler<CreateTeacherCommand, CreateTeacherResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        public CreateTeacherHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CreateTeacherResult> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
        {
            //Check email uniqueness
            var existingUser = _unitOfWork
                .GetRepository<User>().Single(x => x.Email == request.Email);

            if (existingUser != null)
                throw new ArgumentException(ValidationMessages.EMAIL_ALREADY_EXISTS);

            //Get Teacher Role
            var teacherRole = _unitOfWork.GetRepository<UserRoles>()
                .Single(x => x.Name == "Teacher");

            if (teacherRole == null)
                throw new ArgumentException(ValidationMessages.ROLE_NOT_FOUND);

            //Create User (Login)
            var user = _mapper.Map<User>(request);
            user.Password = _passwordHasher.HashPassword(request.Password);
            user.UserRoleId = teacherRole.Id;

            _unitOfWork.GetRepository<User>().Insert(user);
            _unitOfWork.SaveChanges(); // Needed to generate UserId

            //Create Teacher
            var teacher = _mapper.Map<Teacher>(request);
            teacher.UserId = user.Id;

            _unitOfWork.GetRepository<Teacher>().Insert(teacher);
            _unitOfWork.SaveChanges();

            return new CreateTeacherResult(teacher.Id);
        }
    }
}
