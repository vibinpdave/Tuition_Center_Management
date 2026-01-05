namespace TCM.Application.Features.Parents.Commands.HardDelete
{
    public record HardDeleteParentCommand(long Id) : IRequest<HardDeleteParentResult>;
    public record HardDeleteParentResult(bool Success);
    public class HardDeleteParentValidator : AbstractValidator<HardDeleteParentCommand>
    {
        public HardDeleteParentValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.ID_REQUIRED);
        }
    }
    internal class HardDeleteParentHandler : IRequestHandler<HardDeleteParentCommand, HardDeleteParentResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public HardDeleteParentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<HardDeleteParentResult> Handle(HardDeleteParentCommand request, CancellationToken cancellationToken)
        {
            var parent = await _unitOfWork.GetRepository<Parent>().GetByIdAsync(
               request.Id,
               t => t.User
               );

            if (parent == null)
                throw new ArgumentException(ValidationMessages.PARENT_NOT_FOUND);

            _unitOfWork.GetRepository<Parent>().Delete(parent);

            if (parent.User != null)
                _unitOfWork.GetRepository<Parent>().Delete(parent.User);

            _unitOfWork.SaveChanges();

            return new HardDeleteParentResult(true);
        }
    }
}
