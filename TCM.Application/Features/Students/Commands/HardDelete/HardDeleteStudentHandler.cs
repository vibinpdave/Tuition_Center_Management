namespace TCM.Application.Features.Students.Commands.HardDelete
{
    public record HardDeleteStudentCommand(long Id) : IRequest<HardDeleteStudentResult>;
    public record HardDeleteStudentResult(bool Success);
    public class HardDeleteStudentValidator : AbstractValidator<HardDeleteStudentCommand>
    {
        public HardDeleteStudentValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.ID_REQUIRED);
        }
    }
    internal class HardDeleteStudentHandler : IRequestHandler<HardDeleteStudentCommand, HardDeleteStudentResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public HardDeleteStudentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<HardDeleteStudentResult> Handle(HardDeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.GetRepository<Student>().GetByIdAsync(
               request.Id,
               t => t.User
            );

            if (student == null)
                throw new ArgumentException(ValidationMessages.STUDENT_NOT_FOUND);

            _unitOfWork.GetRepository<Student>().Delete(student);

            if (student.User != null)
                _unitOfWork.GetRepository<Student>().Delete(student.User);

            _unitOfWork.SaveChanges();

            return new HardDeleteStudentResult(true);
        }
    }
}
