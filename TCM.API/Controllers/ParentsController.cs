using TCM.Application.Features.Parents.Commands.Create;
using TCM.Application.Features.Parents.Commands.HardDelete;
using TCM.Application.Features.Parents.Commands.SoftDelete;
using TCM.Application.Features.Parents.Commands.Update;
using TCM.Application.Features.Parents.Queries.GetAllParents;
using TCM.Application.Features.Parents.Queries.GetParentById;

namespace TCM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParentsController(IMediator mediator)
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

        [HttpPost]
        public async Task<IActionResult> CreateParent(
        [FromBody] CreateParentCommand command)
        {
            var parentId = await _mediator.Send(command);
            return Ok(parentId);
        }

        #region UpdateParent
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateParent(
            long id,
            [FromBody] UpdateParentCommand command)
        {
            if (id != command.Id)
                return BadRequest("Parent ID mismatch");

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        #endregion

        #region SoftDeleteParent
        [HttpPatch("{id:long}")]
        public async Task<IActionResult> SoftDeleteParent(long id)
        {
            var result = await _mediator.Send(new SoftDeleteParentCommand(id));
            return Ok(result);
        }
        #endregion

        #region HardDeleteParent
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> HardDeleteParent(long id)
        {
            var result = await _mediator.Send(new HardDeleteParentCommand(id));
            return Ok(result);
        }
        #endregion

        #region GetParentById
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetParentById(long id)
        {
            var result = await _mediator.Send(new GetParentByIdQuery(id));
            return Ok(result);
        }
        #endregion

        #region GetParentsPaged
        [HttpGet]
        public async Task<IActionResult> GetParentsPaged(
            [FromQuery] GetAllParentsPagedQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        #endregion
    }
}
