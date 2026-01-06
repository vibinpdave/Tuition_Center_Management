namespace TCM.Application.Features.Students.Commands.Update
{
    public record UpdateStudentCommand(
        long StudentId,
        string Name,
        string MobileNumber,
        string Email,
        string ResidentialAddress,
        string City,
        string State,
        string Country,
        string DataFields,
        long ParentId,
        long GradeId) : IRequest<UpdateStudentResult>;
    public record UpdateStudentResult(bool IsSuccess);

    public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentCommandValidator()
        {
            RuleFor(x => x.StudentId).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.MobileNumber)
                .Matches(RegexPatterns.MOBILE_NUMBER);
            RuleFor(x => x.ParentId).GreaterThan(0);
            RuleFor(x => x.GradeId).GreaterThan(0);
        }
    }
    internal class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand, UpdateStudentResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UpdateStudentHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateStudentResult> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            // Load Student with User
            var studentRepo = _unitOfWork.GetRepository<Student>();

            var studentDetails = await studentRepo.GetByIdAsync(request.StudentId, t => t.User);

            if (studentDetails == null)
                throw new ArgumentException(ValidationMessages.STUDENT_NOT_FOUND);

            // Email change check
            if (!string.Equals(studentDetails.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailExists = _unitOfWork.GetRepository<User>().Single(
                    u => u.Email == request.Email && u.Id != studentDetails.UserId);

                if (emailExists != null)
                    throw new ArgumentException(ValidationMessages.EMAIL_ALREADY_EXISTS);

                // Update User email
                var userDetails = await _unitOfWork.GetRepository<User>().GetByIdAsync(studentDetails.UserId);

                if (userDetails != null)
                    userDetails.Email = request.Email;

                _unitOfWork.GetRepository<User>().Update(userDetails);
            }

            // Update Student fields
            _mapper.Map(request, studentDetails);

            studentRepo.Update(studentDetails);

            _unitOfWork.SaveChanges();

            return new UpdateStudentResult(true);
        }
    }
}
