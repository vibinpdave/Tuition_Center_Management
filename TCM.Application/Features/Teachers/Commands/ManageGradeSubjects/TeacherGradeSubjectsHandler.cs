namespace TCM.Application.Features.Teachers.Commands.ManageGradeSubjects
{
    public record TeacherGradeSubjectsCommand(
        long Id, IReadOnlyCollection<long> GradeSubjectIds) : IRequest<TeacherGradeSubjectsResult>;
    public record TeacherGradeSubjectsResult(bool IsSuccess);

    public class TeacherGradeSubjectsValidator : AbstractValidator<TeacherGradeSubjectsCommand>
    {
        public TeacherGradeSubjectsValidator()
        {
            RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.ID_GREATER_THAN_ZERO);

            RuleFor(x => x.GradeSubjectIds)
                .NotNull()
                .WithMessage(ValidationMessages.REQUIRED_COLLECTION);
        }
    }
    internal class TeacherGradeSubjectsHandler : IRequestHandler<TeacherGradeSubjectsCommand, TeacherGradeSubjectsResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public TeacherGradeSubjectsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<TeacherGradeSubjectsResult> Handle(TeacherGradeSubjectsCommand request, CancellationToken cancellationToken)
        {
            // Validate Teacher exists
            var teacherRepo = _unitOfWork.GetRepository<Teacher>();
            var teacher = await teacherRepo.GetByIdAsync(request.Id);

            if (teacher == null)
                throw new ArgumentException(ValidationMessages.TEACHER_NOT_FOUND);

            // Validate GradeSubjects exist
            var gradeSubjectRepo = _unitOfWork.GetRepository<GradeSubjects>();

            var distinctIds = request.GradeSubjectIds.Distinct().ToList();

            var validGradeSubjectIds = gradeSubjectRepo
                .GetAll(gs => distinctIds.Contains(gs.Id))
                .Select(gs => gs.Id)
                .ToList();

            if (validGradeSubjectIds.Count != distinctIds.Count)
                throw new ArgumentException(ValidationMessages.INVALID_GRADE_SUBJECT);

            // Get existing assignments
            var teacherGradeSubjectRepo = _unitOfWork.GetRepository<TeacherGradeSubjects>();

            var existingAssignments = teacherGradeSubjectRepo
                .GetAll(tgs => tgs.TeacherId == request.Id)
                .ToList();

            var existingIds = existingAssignments.Select(x => x.GradeSubjectId).ToList();

            // Calculate diff
            var toAdd = distinctIds.Except(existingIds).ToList();
            var toRemove = existingIds.Except(distinctIds).ToList();

            // Add new assignments
            foreach (var gradeSubjectId in toAdd)
            {
                teacherGradeSubjectRepo.Insert(new TeacherGradeSubjects
                {
                    TeacherId = request.Id,
                    GradeSubjectId = gradeSubjectId
                });
            }

            //Remove old assignments
            foreach (var entity in existingAssignments
                         .Where(x => toRemove.Contains(x.GradeSubjectId)))
            {
                teacherGradeSubjectRepo.Delete(entity); // hard delete                    
            }

            // Save once
            _unitOfWork.SaveChanges();

            return new TeacherGradeSubjectsResult(true);
        }
    }
}
