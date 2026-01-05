using static TCM.Domain.Enum.Enums;

namespace TCM.Application.Features.Students.Commands.SoftDelete
{
    public record SoftDeleteStudentCommand(long Id) : IRequest<SoftDeleteStudentResult>;
    public record SoftDeleteStudentResult(bool Success);
    public class SoftDeleteStudentValidator : AbstractValidator<SoftDeleteStudentCommand>
    {
        public SoftDeleteStudentValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.ID_REQUIRED);
        }
    }
    internal class SoftDeleteStudentHandler : IRequestHandler<SoftDeleteStudentCommand, SoftDeleteStudentResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public SoftDeleteStudentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SoftDeleteStudentResult> Handle(SoftDeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var studentRepo = _unitOfWork.GetRepository<Student>();
            var userRepo = _unitOfWork.GetRepository<User>();

            var studentObj = await _unitOfWork.GetRepository<Student>().GetByIdAsync(
                request.Id, t => t.User);

            if (studentObj == null)
                throw new ArgumentException(ValidationMessages.STUDENT_NOT_FOUND);

            // Soft delete teacher
            studentObj.Status = (int)Status.Deleted;

            // Disable login
            if (studentObj.User != null)
            {
                studentObj.User.Status = (int)Status.Deleted;
                userRepo.Update(studentObj.User);
            }

            studentRepo.Update(studentObj);

            _unitOfWork.SaveChanges();

            return new SoftDeleteStudentResult(true);
        }
    }
}
