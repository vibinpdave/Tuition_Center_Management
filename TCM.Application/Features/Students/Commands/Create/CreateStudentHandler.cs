namespace TCM.Application.Features.Students.Commands.Create
{
    public record CreateStudentCommand(
        string Name,
        string MobileNumber,
        string Email,
        string ResidentialAddress,
        string City,
        string State,
        string Country,
        long ParentId,
        long GradeId,
        string Password) : IRequest<CreateStudentResult>;
    public record CreateStudentResult(long Id);

    public class CreateStudentValidator : AbstractValidator<CreateStudentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateStudentValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.NAME_REQUIRED);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress().WithMessage(ValidationMessages.EMAIL_REQUIRED);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MustAsync(async (email, ct) =>
                {
                    return !_unitOfWork.GetRepository<User>()
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

            RuleFor(x => x.ParentId)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.PARENT_ID_REQUIRED)
                .MustAsync(ParentExists).WithMessage(ValidationMessages.PARENT_NOT_FOUND);

            RuleFor(x => x.GradeId)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.GRADE_ID_REQUIRED)
                .MustAsync(GradeExists).WithMessage(ValidationMessages.GRADE_NOT_FOUND);
        }

        private async Task<bool> ParentExists(long parentId, CancellationToken token)
        {
            var parent = await _unitOfWork.GetRepository<Parent>().GetByIdAsync(parentId);
            return parent != null;
        }

        private async Task<bool> GradeExists(long gradeId, CancellationToken token)
        {
            var grade = await _unitOfWork.GetRepository<Grade>().GetByIdAsync(gradeId);
            return grade != null;
        }
    }
    internal class CreateStudentHandler : IRequestHandler<CreateStudentCommand, CreateStudentResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        public CreateStudentHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<CreateStudentResult> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            //Create User
            var studentRole = _unitOfWork.GetRepository<UserRoles>().GetAll()
                .FirstOrDefault(r => r.Name.Equals("Student", StringComparison.OrdinalIgnoreCase));

            if (studentRole == null)
                throw new Exception(ValidationMessages.ROLE_NOT_FOUND);

            var user = _mapper.Map<User>(request);
            user.Password = _passwordHasher.HashPassword(request.Password);
            user.UserRoleId = studentRole.Id;

            _unitOfWork.GetRepository<User>().Insert(user);
            _unitOfWork.SaveChanges(); // get user.Id

            var student = _mapper.Map<Student>(request);
            student.UserId = user.Id;

            _unitOfWork.GetRepository<Student>().Insert(student);
            _unitOfWork.SaveChanges();

            return new CreateStudentResult(student.Id);
        }
    }
}
