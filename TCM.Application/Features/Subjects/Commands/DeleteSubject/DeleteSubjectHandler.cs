namespace TCM.Application.Features.Subjects.Commands.DeleteSubject
{
    public record DeleteSubjectCommand(long Id) : IRequest<DeleteSubjectResult>;
    public record DeleteSubjectResult(bool IsSuccess);
    public class DeleteProductCommandValidator : AbstractValidator<DeleteSubjectCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage(string.Format(
                ValidationMessages.ID_REQUIRED,
                nameof(Subject)
            ));
        }
    }
    internal class DeleteSubjectHandler : IRequestHandler<DeleteSubjectCommand, DeleteSubjectResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteSubjectHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DeleteSubjectResult> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.GetRepository<Subject>().SetAsDeleted(request.Id);
            return new DeleteSubjectResult(true);
        }
    }
}
