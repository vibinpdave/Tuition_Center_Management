using static TCM.Domain.Enum.Enums;

namespace TCM.Application.Features.Parents.Commands.SoftDelete
{
    public record SoftDeleteParentCommand(long Id) : IRequest<SoftDeleteParentResult>;
    public record SoftDeleteParentResult(bool Success);
    public class SoftDeleteParentValidator : AbstractValidator<SoftDeleteParentCommand>
    {
        public SoftDeleteParentValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.ID_REQUIRED);
        }
    }
    internal class SoftDeleteParentHandler : IRequestHandler<SoftDeleteParentCommand, SoftDeleteParentResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public SoftDeleteParentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SoftDeleteParentResult> Handle(SoftDeleteParentCommand request, CancellationToken cancellationToken)
        {
            var parentRepo = _unitOfWork.GetRepository<Parent>();
            var userRepo = _unitOfWork.GetRepository<User>();

            var parent = await _unitOfWork.GetRepository<Parent>().GetByIdAsync(
                request.Id, t => t.User);

            if (parent == null)
                throw new ArgumentException(ValidationMessages.PARENT_NOT_FOUND);

            // Soft delete teacher
            parent.Status = (int)Status.Deleted;

            // Disable login
            if (parent.User != null)
            {
                parent.User.Status = (int)Status.Deleted;
                userRepo.Update(parent.User);
            }

            parentRepo.Update(parent);

            _unitOfWork.SaveChanges();

            return new SoftDeleteParentResult(true);
        }
    }
}
