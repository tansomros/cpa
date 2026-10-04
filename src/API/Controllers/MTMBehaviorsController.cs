using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.MTMBehaviors.Commands.Create;
using BigLion.CPA.Application.Features.MTMBehaviors.Commands.Delete;
using BigLion.CPA.Application.Features.MTMBehaviors.Commands.Update;
using BigLion.CPA.Application.Features.MTMBehaviors.Queries.Get;
using BigLion.CPA.Application.Features.MTMBehaviors.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class MTMBehaviorsController : BaseController
{
    [HttpPost(Name = "CreateMTMBehavior")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateMTMBehaviorCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetMTMBehavior")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MTMBehaviorViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<MTMBehaviorViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetMTMBehaviorQuery { UID = id }));
    }

    [HttpGet(Name = "GetMTMBehaviorList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<MTMBehaviorViewModel>))]
    public async Task<ActionResult<PaginatedList<MTMBehaviorViewModel>>> List([FromQuery] GetMTMBehaviorListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateMTMBehavior")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMTMBehaviorCommand command)
    {
        if (id != command.UID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteMTMBehavior")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteMTMBehaviorCommand { UID = id });
        return NoContent();
    }
}
