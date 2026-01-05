namespace TCM.Application.Features.Teachers.Queries.GetTeacherById
{
    public record GetTeacherByIdQuery(long Id) : IRequest<GetTeacherByIdResult>;
    public record GetTeacherByIdResult(TeacherDTO Teacher);

    public class GetTeacherByIdValidator : AbstractValidator<GetTeacherByIdQuery>
    {
        public GetTeacherByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.ID_GREATER_THAN_ZERO);
        }
    }
    internal class GetTeacherByIdHandler : IRequestHandler<GetTeacherByIdQuery, GetTeacherByIdResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetTeacherByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetTeacherByIdResult> Handle(GetTeacherByIdQuery request, CancellationToken cancellationToken)
        {
            var teacher = await _unitOfWork
                .GetRepository<Teacher>()
                .GetByIdAsync(request.Id, t => t.User, t => t.TeacherGradeSubjects);

            if (teacher == null)
                throw new ArgumentException(ValidationMessages.TEACHER_NOT_FOUND);

            var response = _mapper.Map<TeacherDTO>(teacher);

            return new GetTeacherByIdResult(response);
        }
    }
}
