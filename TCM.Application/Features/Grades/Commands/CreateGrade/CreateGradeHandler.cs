namespace TCM.Application.Features.Grades.Commands.CreateGrade
{
    public record CreateGradeCommand(string Name, string Description) : IRequest<CreateGradeResult>;
    public record CreateGradeResult(long Id);

    #region CreateGradeValidator
    public class CreateGradeValidator : AbstractValidator<CreateGradeCommand>
    {
        public CreateGradeValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(
                ValidationMessages.NAME_REQUIRED,
                nameof(Grade)
            ));
        }
    }
    #endregion

    #region CreateGradeHandler
    internal class CreateGradeHandler : IRequestHandler<CreateGradeCommand, CreateGradeResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateGradeHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<CreateGradeResult> Handle(CreateGradeCommand request, CancellationToken cancellationToken)
        {
            var objGrade = _mapper.Map<Grade>(request);
            _unitOfWork.GetRepository<Grade>().Insert(objGrade);
            _unitOfWork.SaveChanges();

            var response = new CreateGradeResult(objGrade.Id);

            return response;
        }
    }
    #endregion
}
