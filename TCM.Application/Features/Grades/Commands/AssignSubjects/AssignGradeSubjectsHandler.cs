namespace TCM.Application.Features.Grades.Commands.AssignSubjects
{
    public record AssignGradeSubjectsCommand(long GradeId, IReadOnlyList<long> SubjectIds) : IRequest<AssignGradeSubjectsResult>;
    public record AssignGradeSubjectsResult(bool IsSuccess);

    public class AssignGradeSubjectsValidator : AbstractValidator<AssignGradeSubjectsCommand>
    {
        public AssignGradeSubjectsValidator()
        {
            RuleFor(x => x.GradeId)
                    .GreaterThan(0)
                    .WithMessage(
                        string.Format(ValidationMessages.ID_REQUIRED, "Grade")
                        );

            RuleFor(x => x.SubjectIds)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage(ValidationMessages.ASSIGN_AT_LEAST_ONE_SUBJECT);

            RuleForEach(x => x.SubjectIds)
                    .GreaterThan(0)
                    .WithMessage(ValidationMessages.INVALID_SUBJECT_ID);

        }
    }
    internal class AssignGradeSubjectsHandler : IRequestHandler<AssignGradeSubjectsCommand, AssignGradeSubjectsResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AssignGradeSubjectsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AssignGradeSubjectsResult> Handle(AssignGradeSubjectsCommand request, CancellationToken cancellationToken)
        {
            // Check grade exists
            var grade = await _unitOfWork.GetRepository<Grade>().GetByIdAsync(request.GradeId);

            if (grade == null)
                throw new ArgumentException(
                    string.Format(ValidationMessages.GRADE_NOT_FOUND)
                );

            // Fetch already assigned subjects
            var existingAssignments = _unitOfWork.GetRepository<GradeSubjects>()
                .GetAll(gs => gs.GradeId == request.GradeId)
                .Select(gs => gs.SubjectId)
                .ToHashSet();

            //Add only new subjects
            foreach (var subjectId in request.SubjectIds.Distinct())
            {
                if (!existingAssignments.Contains(subjectId))
                {
                    _unitOfWork.GetRepository<GradeSubjects>().Insert(new GradeSubjects
                    {
                        GradeId = request.GradeId,
                        SubjectId = subjectId
                    });
                }
            }

            // Save changes
            _unitOfWork.SaveChanges();

            return new AssignGradeSubjectsResult(true);
        }
    }
}
