namespace TCM.Application.Features.Parents.Queries.GetParentById
{
    public record GetParentByIdQuery(long Id) : IRequest<GetParentByIdResult>;
    public record GetParentByIdResult(ParentDTO Parent);
    public class GetParentByIdValidator : AbstractValidator<GetParentByIdQuery>
    {
        public GetParentByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.ID_REQUIRED);
        }
    }
    internal class GetParentByIdHandler : IRequestHandler<GetParentByIdQuery, GetParentByIdResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetParentByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetParentByIdResult> Handle(GetParentByIdQuery request, CancellationToken cancellationToken)
        {
            var parent = await _unitOfWork.GetRepository<Parent>()
                .GetByIdAsync(request.Id, p => p.User);

            if (parent == null)
                throw new ArgumentException(ValidationMessages.PARENT_NOT_FOUND);

            var dto = _mapper.Map<ParentDTO>(parent);

            return new GetParentByIdResult(dto);
        }
    }
}
