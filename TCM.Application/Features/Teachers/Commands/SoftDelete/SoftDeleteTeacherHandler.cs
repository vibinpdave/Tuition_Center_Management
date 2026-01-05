using static TCM.Domain.Enum.Enums;

namespace TCM.Application.Features.Teachers.Commands.SoftDelete
{
    public record SoftDeleteTeacherCommand(long Id) : IRequest<SoftDeleteTeacherResult>;
    public record SoftDeleteTeacherResult(bool Success);
    public class SoftDeleteTeacherValidator : AbstractValidator<SoftDeleteTeacherCommand>
    {
        public SoftDeleteTeacherValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.ID_REQUIRED);
        }
    }
    internal class SoftDeleteTeacherHandler : IRequestHandler<SoftDeleteTeacherCommand, SoftDeleteTeacherResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public SoftDeleteTeacherHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<SoftDeleteTeacherResult> Handle(SoftDeleteTeacherCommand request, CancellationToken cancellationToken)
        {
            var teacherRepo = _unitOfWork.GetRepository<Teacher>();
            var userRepo = _unitOfWork.GetRepository<User>();

            var teacher = await _unitOfWork.GetRepository<Teacher>().GetByIdAsync(
                request.Id,
                t => t.User);

            if (teacher == null)
                throw new ArgumentException("Teacher not found");

            // Soft delete teacher
            teacher.Status = (int)Status.Deleted;

            // Disable login
            if (teacher.User != null)
            {
                teacher.User.Status = (int)Status.Deleted;
                teacher.User.DateModified = DateTime.UtcNow;
                userRepo.Update(teacher.User);
            }

            teacherRepo.Update(teacher);

            _unitOfWork.SaveChanges();

            return new SoftDeleteTeacherResult(true);
        }
    }
}
