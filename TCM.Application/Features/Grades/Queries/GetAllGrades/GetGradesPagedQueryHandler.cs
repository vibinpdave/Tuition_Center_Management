namespace TCM.Application.Features.Grades.Queries.GetAllGrades
{
    public record GetGradesPagedQuery(
        int PageIndex = 1,
        int PageSize = 20,
        string Search = null,
        string SortBy = null,
        string SortOrder = null) : IRequest<GetGradesPagedResult>;
    public record GetGradesPagedResult(int TotalRecords, IReadOnlyList<GradeDTO> Grades);

    #region GetGradesPagedQueryValidator
    public class GetGradesPagedQueryValidator : AbstractValidator<GetGradesPagedQuery>
    {
        public GetGradesPagedQueryValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage(ValidationMessages.PAGE_NUMBER_GREATER_THAN_ZERO);

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage(ValidationMessages.PAGE_SIZE_GREATER_THAN_ZERO);

            RuleFor(x => x.SortOrder)
                .Must(x => x == "asc" || x == "desc")
                .When(x => !string.IsNullOrWhiteSpace(x.SortOrder));
        }
    }
    #endregion
    internal class GetGradesPagedQueryHandler : IRequestHandler<GetGradesPagedQuery, GetGradesPagedResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetGradesPagedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GetGradesPagedResult> Handle(GetGradesPagedQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Grade, bool>> filter = null;

            var normalizedSortColumn = SortColumnHelper<Subject>.Normalize(request.SortBy);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                filter = s =>
                    s.Name.Contains(request.Search) ||
                    s.Description.Contains(request.Search);
            }

            Func<IQueryable<Grade>, IOrderedQueryable<Grade>> orderBy = request.SortOrder == "desc"
                ? q => q.OrderByDescending(e => EF.Property<object>(e, normalizedSortColumn))
                : q => q.OrderBy(e => EF.Property<object>(e, normalizedSortColumn));

            var pagedResult = _unitOfWork.GetRepository<Grade>()
                .GetAllPaged(
                    filter: filter,
                    orderBy: orderBy,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize
                    );

            var gradetDtos = _mapper.Map<IReadOnlyList<GradeDTO>>(pagedResult.Results);

            return new GetGradesPagedResult(pagedResult.Count, gradetDtos);
        }
    }
}
