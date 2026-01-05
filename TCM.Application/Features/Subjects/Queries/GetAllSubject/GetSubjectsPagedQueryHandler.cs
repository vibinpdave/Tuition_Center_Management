namespace TCM.Application.Features.Subjects.Queries.GetAllSubject
{
    public record GetSubjectsPagedQuery(
        int PageIndex = 1,
        int PageSize = 20,
        string Search = null,
        string SortBy = null,
        string SortOrder = null) : IRequest<GetSubjectsPagedResult>;
    public record GetSubjectsPagedResult(int TotalRecords, IReadOnlyList<SubjectDTO> Subjects);

    #region GetSubjectsPagedQueryValidator
    public class GetSubjectsPagedQueryValidator : AbstractValidator<GetSubjectsPagedQuery>
    {
        public GetSubjectsPagedQueryValidator()
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

    #region GetSubjectsPagedQueryHandler
    internal class GetSubjectsPagedQueryHandler : IRequestHandler<GetSubjectsPagedQuery, GetSubjectsPagedResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetSubjectsPagedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GetSubjectsPagedResult> Handle(GetSubjectsPagedQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Subject, bool>> filter = null;

            var normalizedSortColumn = SortColumnHelper<Subject>.Normalize(request.SortBy);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                filter = s =>
                    s.Name.Contains(request.Search) ||
                    s.Description.Contains(request.Search);
            }

            Func<IQueryable<Subject>, IOrderedQueryable<Subject>> orderBy = request.SortOrder == "desc"
                ? q => q.OrderByDescending(e => EF.Property<object>(e, normalizedSortColumn))
                : q => q.OrderBy(e => EF.Property<object>(e, normalizedSortColumn));

            var pagedResult = _unitOfWork.GetRepository<Subject>()
                .GetAllPaged(
                    filter: filter,
                    orderBy: orderBy,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize
                    );

            var subjectDtos = _mapper.Map<IReadOnlyList<SubjectDTO>>(pagedResult.Results);

            return new GetSubjectsPagedResult(pagedResult.Count, subjectDtos);
        }
    }
    #endregion
}
