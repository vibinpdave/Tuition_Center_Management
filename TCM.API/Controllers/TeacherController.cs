using TCM.Application.Features.Teachers.Commands.Create;
using TCM.Application.Features.Teachers.Commands.HardDelete;
using TCM.Application.Features.Teachers.Commands.ManageGradeSubjects;
using TCM.Application.Features.Teachers.Commands.SoftDelete;
using TCM.Application.Features.Teachers.Commands.Update;
using TCM.Application.Features.Teachers.Queries.GetAllTeachers;
using TCM.Application.Features.Teachers.Queries.GetTeacherById;

namespace TCM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TeacherController(IMediator mediator)
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

        #region CreateTeacher
        [HttpPost]
        public async Task<IActionResult> CreateTeacher(
        [FromBody] CreateTeacherCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        #endregion

        #region UpdateTeacher
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateTeacher(
            long id,
            [FromBody] UpdateTeacherCommand command)
        {
            if (id != command.Id)
                return BadRequest("Teacher ID mismatch");

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        #endregion

        #region SoftDeleteTeacher
        [HttpPatch("{id:long}")]
        public async Task<IActionResult> SoftDeleteTeacher(long id)
        {
            var result = await _mediator.Send(new SoftDeleteTeacherCommand(id));
            return Ok(result);
        }
        #endregion

        #region HardDeleteTeacher
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> HardDeleteTeacher(long id)
        {
            var result = await _mediator.Send(new HardDeleteTeacherCommand(id));
            return Ok(result);
        }
        #endregion

        #region SyncGradeSubjects
        [HttpPut("{teacherId:long}/grade-subjects")]
        public async Task<IActionResult> SyncGradeSubjects(
            long teacherId,
            [FromBody] TeacherGradeSubjectsCommand request)
        {
            var command = new TeacherGradeSubjectsCommand(
                teacherId,
                request.GradeSubjectIds
            );

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        #endregion

        #region GetTeacherById
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetTeacherById(long id)
        {
            var result = await _mediator.Send(
                new GetTeacherByIdQuery(id));

            return Ok(result);
        }
        #endregion

        #region GetTeachersPaged
        [HttpGet]
        public async Task<IActionResult> GetTeachersPaged(
            [FromQuery] GetAllTeachersPagedQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        #endregion
    }
}
