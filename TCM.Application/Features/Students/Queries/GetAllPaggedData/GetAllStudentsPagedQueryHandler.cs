namespace TCM.Application.Features.Students.Queries.GetAllPaggedData
{
    public record GetAllStudentsPagedQuery(
        int PageIndex,
        int PageSize,
        string Search,
        string SortBy = "Name",
        string SortOrder = "asc") : IRequest<GetAllStudentsPagedResult>;
    public record GetAllStudentsPagedResult(
        int TotalCount,
        IReadOnlyList<StudentDTO> Parents
    );
    public class GetAllStudentsPagedQueryValidator : AbstractValidator<GetAllStudentsPagedQuery>
    {
        public GetAllStudentsPagedQueryValidator()
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
    internal class GetAllStudentsPagedQueryHandler : IRequestHandler<GetAllStudentsPagedQuery, GetAllStudentsPagedResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllStudentsPagedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetAllStudentsPagedResult> Handle(GetAllStudentsPagedQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Student, bool>> filter = null;

            var normalizedSortColumn = SortColumnHelper<Student>.Normalize(request.SortBy);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                filter = p =>
                    p.Name.Contains(request.Search) ||
                    p.MobileNumber.Contains(request.Search) ||
                    p.User.Email.Contains(request.Search);
            }

            Func<IQueryable<Student>, IOrderedQueryable<Student>> orderBy =
            request.SortOrder == "desc" ? q => q.OrderByDescending(
                e => EF.Property<object>(e, normalizedSortColumn)) : q => q.OrderBy(
                e => EF.Property<object>(e, normalizedSortColumn));

            var pagedResult = _unitOfWork.GetRepository<Student>()
                .GetAllPaged(
                    filter: filter,
                    orderBy: orderBy,
                    include: q => q.Include(p => p.User),
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize
                    );

            var response = _mapper.Map<IReadOnlyList<StudentDTO>>(pagedResult.Results);

            return new GetAllStudentsPagedResult(
                pagedResult.Count,
                response
                );
        }
    }
}
