using TCM.Application.Features.Students.Commands.Create;

namespace TCM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Options
        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Append("Allow", "GET, POST, PUT, PATCH, DELETE, OPTIONS");
            return Ok();
        }
        #endregion

        #region CreateStudent
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        #endregion
    }
}
