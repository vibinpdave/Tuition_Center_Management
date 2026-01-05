
namespace TCM.Application.Features.Teachers.Commands.HardDelete
{
    public record HardDeleteTeacherCommand(long Id) : IRequest<HardDeleteTeacherResult>;
    public record HardDeleteTeacherResult(bool Success);

    public class HardDeleteTeacherValidator : AbstractValidator<HardDeleteTeacherCommand>
    {
        public HardDeleteTeacherValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.ID_REQUIRED);
        }
    }

    internal class HardDeleteTeacherHandler : IRequestHandler<HardDeleteTeacherCommand, HardDeleteTeacherResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public HardDeleteTeacherHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<HardDeleteTeacherResult> Handle(HardDeleteTeacherCommand request, CancellationToken cancellationToken)
        {
            var teacher = await _unitOfWork.GetRepository<Teacher>().GetByIdAsync(
                request.Id,
                t => t.User,
                t => t.TeacherGradeSubjects
                );

            if (teacher == null)
                throw new ArgumentException(ValidationMessages.TEACHER_NOT_FOUND);

            if (teacher.TeacherGradeSubjects?.Any() == true)
            {
                foreach (var tgs in teacher.TeacherGradeSubjects)
                    _unitOfWork.GetRepository<TeacherGradeSubjects>().Delete(tgs);
            }

            _unitOfWork.GetRepository<Teacher>().Delete(teacher);

            if (teacher.User != null)
                _unitOfWork.GetRepository<User>().Delete(teacher.User);

            _unitOfWork.SaveChanges();

            return new HardDeleteTeacherResult(true);
        }
    }
}
