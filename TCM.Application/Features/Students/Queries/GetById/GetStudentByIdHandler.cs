namespace TCM.Application.Features.Students.Queries.GetById
{
    public record GetStudentByIdQuery(long Id) : IRequest<GetStudentByIdResult>;
    public record GetStudentByIdResult(StudentDTO Parent);
    public class GetStudentByIdValidator : AbstractValidator<GetStudentByIdQuery>
    {
        public GetStudentByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.ID_REQUIRED);
        }
    }

    internal class GetStudentByIdHandler : IRequestHandler<GetStudentByIdQuery, GetStudentByIdResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudentByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetStudentByIdResult> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.GetRepository<Student>()
                .GetByIdAsync(request.Id, s => s.User, s => s.Parent, s => s.Grade);

            if (student == null)
                throw new ArgumentException(ValidationMessages.STUDENT_NOT_FOUND);

            var dto = _mapper.Map<StudentDTO>(student);

            return new GetStudentByIdResult(dto);
        }
    }
}
