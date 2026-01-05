namespace TCM.Application.Features.Parents.Queries.GetAllParents
{
    public record GetAllParentsPagedQuery(
        int PageIndex,
        int PageSize,
        string? Search,
        string? SortBy = "Name",
        string? SortOrder = "asc"
    ) : IRequest<GetAllParentsPagedResult>;
    public record GetAllParentsPagedResult(
        int TotalCount,
        IReadOnlyList<ParentListDTO> Parents
    );
    public class GetAllParentsPagedQueryValidator : AbstractValidator<GetAllParentsPagedQuery>
    {
        public GetAllParentsPagedQueryValidator()
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
    internal class GetAllParentsPagedQueryHandler : IRequestHandler<GetAllParentsPagedQuery, GetAllParentsPagedResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllParentsPagedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetAllParentsPagedResult> Handle(GetAllParentsPagedQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Parent, bool>> filter = null;

            var normalizedSortColumn = SortColumnHelper<Parent>.Normalize(request.SortBy);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                filter = p =>
                    p.Name.Contains(request.Search) ||
                    p.MobileNumber.Contains(request.Search) ||
                    p.User.Email.Contains(request.Search);
            }

            Func<IQueryable<Parent>, IOrderedQueryable<Parent>> orderBy =
            request.SortOrder == "desc" ? q => q.OrderByDescending(
                e => EF.Property<object>(e, normalizedSortColumn)) : q => q.OrderBy(
                e => EF.Property<object>(e, normalizedSortColumn));

            var pagedResult = _unitOfWork.GetRepository<Parent>()
                .GetAllPaged(
                    filter: filter,
                    orderBy: orderBy,
                    include: q => q.Include(p => p.User),
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize
                    );

            var response = _mapper.Map<IReadOnlyList<ParentListDTO>>(pagedResult.Results);

            return new GetAllParentsPagedResult(
                pagedResult.Count,
                response
                );
        }
    }
}
