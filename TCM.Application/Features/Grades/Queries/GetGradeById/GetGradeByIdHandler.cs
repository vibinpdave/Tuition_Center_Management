namespace TCM.Application.Features.Grades.Queries.GetGradeById
{
    public record GetGradeByIdQuery(long Id) : IRequest<GetGradeByIdResult>;
    public record GetGradeByIdResult(GradeDTO Subject);

    #region GetGradeByIdValidator
    public class GetGradeByIdValidator : AbstractValidator<GetGradeByIdQuery>
    {
        public GetGradeByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage(string.Format(ValidationMessages.ID_REQUIRED, nameof(Grade)));
        }
    }
    #endregion

    #region GetGradeByIdHandler
    internal class GetGradeByIdHandler : IRequestHandler<GetGradeByIdQuery, GetGradeByIdResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetGradeByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GetGradeByIdResult> Handle(GetGradeByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.GetRepository<Grade>().GetByIdAsync(request.Id);

            // Map Grade → DTO
            var objGradetDto = _mapper.Map<GradeDTO>(result);

            // Wrap DTO in Result
            var response = new GetGradeByIdResult(objGradetDto);

            return response;
        }
    }
    #endregion
}
