namespace TCM.Application.Features.Parents.Commands.Update
{
    public record UpdateParentCommand(
         long Id,
         string Name,
         string MobileNumber,
         string Email,
         string ResidentialAddress,
         string City,
         string State,
         string Country
     ) : IRequest<UpdateParentResult>;
    public record UpdateParentResult(bool Success);
    public class UpdateParentValidator : AbstractValidator<UpdateParentCommand>
    {
        public UpdateParentValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.ID_REQUIRED);

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.NAME_REQUIRED);

            RuleFor(x => x.MobileNumber)
                .NotEmpty()
                .Matches(RegexPatterns.MOBILE_NUMBER)
                .WithMessage(ValidationMessages.INVALID_MOBILE);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
    internal class UpdateParentHandler : IRequestHandler<UpdateParentCommand, UpdateParentResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateParentHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateParentResult> Handle(UpdateParentCommand request, CancellationToken cancellationToken)
        {
            // Get Parent
            var parentDetails = await _unitOfWork.GetRepository<Parent>().GetByIdAsync(request.Id);

            if (parentDetails == null)
                throw new ArgumentException(ValidationMessages.PARENT_NOT_FOUND);

            // Email change check
            if (!string.Equals(parentDetails.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailExists = _unitOfWork.GetRepository<User>().Single(
                    u => u.Email == request.Email && u.Id != parentDetails.UserId);

                if (emailExists != null)
                    throw new ArgumentException(ValidationMessages.EMAIL_ALREADY_EXISTS);

                // Update User email
                var userDetails = await _unitOfWork.GetRepository<User>().GetByIdAsync(parentDetails.UserId);

                if (userDetails != null)
                    userDetails.Email = request.Email;

                _unitOfWork.GetRepository<User>().Update(userDetails);
            }

            // Update Teacher fields
            _mapper.Map(request, parentDetails);
            _unitOfWork.GetRepository<Parent>().Update(parentDetails);

            _unitOfWork.SaveChanges();

            return new UpdateParentResult(true);

        }
    }
}
