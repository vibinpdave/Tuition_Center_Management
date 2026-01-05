namespace TCM.Application.Features.Subjects.Queries.GetSubjectById
{
    public record GetSubjectByIdQuery(long Id) : IRequest<GetSubjectByIdResult>;
    public record GetSubjectByIdResult(SubjectDTO Subject);
    public class GetSubjectByIdValidator : AbstractValidator<GetSubjectByIdQuery>
    {
        public GetSubjectByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Subject ID is required");
        }
    }
    internal class GetSubjectByIdHandler : IRequestHandler<GetSubjectByIdQuery, GetSubjectByIdResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetSubjectByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GetSubjectByIdResult> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.GetRepository<Subject>().GetByIdAsync(request.Id);

            // Map Subject → DTO
            var objSubjectDto = _mapper.Map<SubjectDTO>(result);

            // Wrap DTO in Result
            var response = new GetSubjectByIdResult(objSubjectDto);

            return response;
        }
    }
}
