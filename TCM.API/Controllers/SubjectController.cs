using TCM.Application.Features.Subjects.Commands.CreateSubject;
using TCM.Application.Features.Subjects.Commands.DeleteSubject;
using TCM.Application.Features.Subjects.Queries.GetAllSubject;
using TCM.Application.Features.Subjects.Queries.GetSubjectById;

namespace TCM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        #region Private Variables
        private readonly IMediator _mediator;
        #endregion

        public SubjectController(IMediator mediator)
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

        #region Creates new subject
        /// <summary>
        /// Creates new subject.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Post(CreateSubjectCommand subjectDetails)
        {
            var response = await _mediator.Send(subjectDetails);
            return Ok(response);
        }
        #endregion

        #region Get Subject By Id
        /// <summary>
        /// Get Subject By Id
        /// </summary>
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetSubjectByIdQuery(id));

            return Ok(result);
        }
        #endregion

        #region Get Pagged subjects
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedSubjects(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string search = null,
            [FromQuery] string sortBy = "Name",
            [FromQuery] string sortOrder = "asc")
        {
            var result = await _mediator.Send(
                new GetSubjectsPagedQuery(
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

        #region Delete a subject
        /// <summary>
        /// Delete a subject
        /// </summary>
        [HttpDelete("{id:long}")]
        public async Task<ActionResult> Delete(long id)
        {
            var response = await _mediator.Send(new DeleteSubjectCommand(id));
            return Ok(response);
        }
        #endregion
    }
}
