using TCM.Application.Features.Grades.Commands.AssignSubjects;
using TCM.Application.Features.Grades.Commands.CreateGrade;
using TCM.Application.Features.Grades.Commands.DeleteGrade;
using TCM.Application.Features.Grades.Commands.RemoveGradeSubjects;
using TCM.Application.Features.Grades.Queries.GetAllGrades;
using TCM.Application.Features.Grades.Queries.GetGradeById;

namespace TCM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        #region Private Variables
        private readonly IMediator _mediator;
        #endregion

        public GradeController(IMediator mediator)
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

        #region Creates new Grade
        /// <summary>
        /// Creates new Grade.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Post(CreateGradeCommand gradeDetails)
        {
            var response = await _mediator.Send(gradeDetails);
            return Ok(response);
        }
        #endregion

        #region Get Grade By Id
        /// <summary>
        /// Get Grade By Id
        /// </summary>
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetGradeByIdQuery(id));

            return Ok(result);
        }
        #endregion

        #region Get Pagged Grades
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedGrades(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string search = null,
            [FromQuery] string sortBy = "Name",
            [FromQuery] string sortOrder = "asc")
        {
            var result = await _mediator.Send(
                new GetGradesPagedQuery(
                    PageIndex: pageIndex,
                    PageSize: pageSize,
                    Search: search,
                    SortBy: sortBy,
                    SortOrder: sortOrder
                )
            );

            return Ok(result);
        }
        #endregion

        #region Delete a Grade
        /// <summary>
        /// Delete a Grade
        /// </summary>
        [HttpDelete("{id:long}")]
        public async Task<ActionResult> Delete(long id)
        {
            var response = await _mediator.Send(new DeleteGradeCommand(id));
            return Ok(response);
        }
        #endregion

        #region AssignSubjectsToGrade
        [HttpPost("{gradeId}/subjects")]
        public async Task<IActionResult> AssignSubjectsToGrade(
            long gradeId,
            [FromBody] List<long> SubjectIds)
        {
            var result = await _mediator.Send(
                 new AssignGradeSubjectsCommand(
                    gradeId,
                    SubjectIds
                    )
                );

            return Ok(result);
        }
        #endregion

        #region RemoveSubjectsFromGrade
        [HttpDelete("{gradeId}/subjects")]
        public async Task<IActionResult> RemoveSubjectsFromGrade(
            long gradeId,
            [FromBody] List<long> subjectIds)
        {
            var result = await _mediator.Send(
                new RemoveGradeSubjectsCommand(
                    gradeId,
                    subjectIds
                )
            );

            return Ok(result);
        }
        #endregion
    }
}
