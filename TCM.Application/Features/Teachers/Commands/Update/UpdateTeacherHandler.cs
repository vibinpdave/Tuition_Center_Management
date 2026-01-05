namespace TCM.Application.Features.Teachers.Commands.Update
{
    public record UpdateTeacherCommand(
        long Id,
        string Name,
        string MobileNumber,
        string Email,
        string ResidentialAddress,
        string City,
        string State,
        string Country,
        string Qualification
    ) : IRequest<UpdateTeacherResult>;

    public record UpdateTeacherResult(bool Success);

    public class UpdateTeacherValidator : AbstractValidator<UpdateTeacherCommand>
    {
        public UpdateTeacherValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.ID_REQUIRED);

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(ValidationMessages.NAME_REQUIRED, "Teacher Name"));

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.MobileNumber)
                .NotEmpty();
        }
    }
    internal class UpdateTeacherHandler : IRequestHandler<UpdateTeacherCommand, UpdateTeacherResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateTeacherHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<UpdateTeacherResult> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
        {
            // Get Teacher
            var teacherDetails = await _unitOfWork.GetRepository<Teacher>().GetByIdAsync(request.Id);

            if (teacherDetails == null)
                throw new ArgumentException(ValidationMessages.TEACHER_NOT_FOUND);

            // Email change check
            if (!string.Equals(teacherDetails.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailExists = _unitOfWork.GetRepository<User>().Single(
                    u => u.Email == request.Email && u.Id != teacherDetails.UserId);

                if (emailExists != null)
                    throw new ArgumentException(ValidationMessages.EMAIL_ALREADY_EXISTS);

                // Update User email
                var userDetails = await _unitOfWork.GetRepository<User>().GetByIdAsync(teacherDetails.UserId);

                if (userDetails != null)
                    userDetails.Email = request.Email;

                _unitOfWork.GetRepository<User>().Update(userDetails);
            }

            // Update Teacher fields
            _mapper.Map(request, teacherDetails);
            _unitOfWork.GetRepository<Teacher>().Update(teacherDetails);

            _unitOfWork.SaveChanges();

            return new UpdateTeacherResult(true);

        }
    }
}
