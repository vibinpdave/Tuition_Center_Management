namespace TCM.Application.Features.Grades.Commands.DeleteGrade
{
    public record DeleteGradeCommand(long Id) : IRequest<DeleteGradeResult>;
    public record DeleteGradeResult(bool IsSuccess);

    #region DeleteGradeCommandValidator
    public class DeleteGradeCommandValidator : AbstractValidator<DeleteGradeCommand>
    {
        public DeleteGradeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage(string.Format(
                ValidationMessages.ID_REQUIRED,
                nameof(Grade)
            ));
        }
    }
    #endregion

    #region DeleteGradeHandler
    internal class DeleteGradeHandler : IRequestHandler<DeleteGradeCommand, DeleteGradeResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteGradeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DeleteGradeResult> Handle(DeleteGradeCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.GetRepository<Grade>().SetAsDeleted(request.Id);
            return new DeleteGradeResult(true);
        }
    }
    #endregion
}
