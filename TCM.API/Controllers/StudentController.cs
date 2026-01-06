using TCM.Application.Features.Students.Commands.Create;
using TCM.Application.Features.Students.Commands.HardDelete;
using TCM.Application.Features.Students.Commands.SoftDelete;
using TCM.Application.Features.Students.Commands.Update;
using TCM.Application.Features.Students.Queries.GetAllPaggedData;
using TCM.Application.Features.Students.Queries.GetById;

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

        #region UpdateStudent
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateStudent(
            long id,
            [FromBody] UpdateStudentCommand command)
        {
            if (id != command.StudentId)
                return BadRequest("Student ID mismatch");

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        #endregion

        #region SoftDeleteStudent
        [HttpPatch("{id:long}")]
        public async Task<IActionResult> SoftDeleteStudent(long id)
        {
            var result = await _mediator.Send(new SoftDeleteStudentCommand(id));
            return Ok(result);
        }
        #endregion

        #region HardDeleteStudent
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> HardDeleteStudent(long id)
        {
            var result = await _mediator.Send(new HardDeleteStudentCommand(id));
            return Ok(result);
        }
        #endregion

        #region GetStudentById
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetStudentById(long id)
        {
            var result = await _mediator.Send(new GetStudentByIdQuery(id));
            return Ok(result);
        }
        #endregion

        #region GetStudentsPaged
        [HttpGet]
        public async Task<IActionResult> GetStudentsPaged(
            [FromQuery] GetAllStudentsPagedQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        #endregion
    }
}
