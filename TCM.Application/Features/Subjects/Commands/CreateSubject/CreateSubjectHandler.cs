namespace TCM.Application.Features.Subjects.Commands.CreateSubject
{
    public record CreateSubjectCommand(string Name, string Description) : IRequest<CreateSubjectResult>;
    public record CreateSubjectResult(long Id);

    public class CreateSubjectValidator : AbstractValidator<CreateSubjectCommand>
    {
        public CreateSubjectValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Subject Name is required");
        }
    }
    internal class CreateSubjectHandler : IRequestHandler<CreateSubjectCommand, CreateSubjectResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateSubjectHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CreateSubjectResult> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
        {
            var objSubject = _mapper.Map<Subject>(request);
            _unitOfWork.GetRepository<Subject>().Insert(objSubject);
            _unitOfWork.SaveChanges();

            var response = new CreateSubjectResult(objSubject.Id);
            return response;
        }
    }
}
