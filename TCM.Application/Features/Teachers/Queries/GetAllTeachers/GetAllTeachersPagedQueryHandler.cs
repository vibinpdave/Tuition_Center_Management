namespace TCM.Application.Features.Teachers.Queries.GetAllTeachers
{
    public record GetAllTeachersPagedQuery(
        int PageIndex = 1,
        int PageSize = 20,
        string Search = null,
        string SortBy = null,
        string SortOrder = null) : IRequest<GetAllTeachersPagedResult>;
    public record GetAllTeachersPagedResult(int TotalRecords, IReadOnlyList<TeacherListDTO> Subjects);

    public class GetAllTeachersPagedQueryValidator : AbstractValidator<GetAllTeachersPagedQuery>
    {
        public GetAllTeachersPagedQueryValidator()
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
    internal class GetAllTeachersPagedQueryHandler : IRequestHandler<GetAllTeachersPagedQuery, GetAllTeachersPagedResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllTeachersPagedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetAllTeachersPagedResult> Handle(GetAllTeachersPagedQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Teacher, bool>> filter = null;

            var normalizedSortColumn = SortColumnHelper<Teacher>.Normalize(request.SortBy);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                filter = t =>
                    t.Name.Contains(request.Search) ||
                    t.MobileNumber.Contains(request.Search) ||
                    t.User.Email.Contains(request.Search);
            }

            Func<IQueryable<Teacher>, IOrderedQueryable<Teacher>> orderBy =
            request.SortOrder == "desc" ? q => q.OrderByDescending(
                e => EF.Property<object>(e, normalizedSortColumn)) :
                q => q.OrderBy(e => EF.Property<object>(e, normalizedSortColumn));

            var pagedResult = _unitOfWork
                .GetRepository<Teacher>()
                .GetAllPaged(
                    filter: filter,
                    orderBy: orderBy,
                    include: q => q.Include(t => t.User),
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize
                );

            var response = _mapper.Map<IReadOnlyList<TeacherListDTO>>(pagedResult.Results);

            return new GetAllTeachersPagedResult(
                pagedResult.Count,
                response
                );
        }
    }
}
