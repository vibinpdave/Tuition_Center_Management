namespace TCM.Application.Features.Grades.Commands.RemoveGradeSubjects
{
    public record RemoveGradeSubjectsCommand(long GradeId, IReadOnlyList<long> SubjectIds) : IRequest<RemoveGradeSubjectsResult>;
    public record RemoveGradeSubjectsResult(bool IsSuccess);

    public class RemoveGradeSubjectsValidator
    : AbstractValidator<RemoveGradeSubjectsCommand>
    {
        public RemoveGradeSubjectsValidator()
        {
            RuleFor(x => x.GradeId)
                .GreaterThan(0)
                .WithMessage(
                string.Format(ValidationMessages.ID_REQUIRED, "Grade")
            );

            RuleFor(x => x.SubjectIds)
                .NotNull()
                .NotEmpty()
                .WithMessage(ValidationMessages.REMOVE_AT_LEAST_ONE_SUBJECT);

            RuleForEach(x => x.SubjectIds)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.INVALID_SUBJECT_ID);
        }
    }
    internal class RemoveGradeSubjectsHandler : IRequestHandler<RemoveGradeSubjectsCommand, RemoveGradeSubjectsResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveGradeSubjectsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<RemoveGradeSubjectsResult> Handle(RemoveGradeSubjectsCommand request, CancellationToken cancellationToken)
        {
            // Check grade exists
            var grade = await _unitOfWork.GetRepository<Grade>()
                .GetByIdAsync(request.GradeId);

            if (grade == null)
                throw new ArgumentException(ValidationMessages.GRADE_NOT_FOUND);

            // Get GradeSubjects to remove
            var gradeSubjects = _unitOfWork.GetRepository<GradeSubjects>()
                .GetAll(gs => gs.GradeId == request.GradeId
                        && request.SubjectIds.Contains(gs.SubjectId));

            foreach (var gs in gradeSubjects)
            {
                //Hard delete
                _unitOfWork.GetRepository<GradeSubjects>().Delete(gs);
            }

            _unitOfWork.SaveChanges();

            return new RemoveGradeSubjectsResult(true);
        }
    }
}
