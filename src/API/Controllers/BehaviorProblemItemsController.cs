using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.BehaviorProblemItems.Commands.Create;
using BigLion.CPA.Application.Features.BehaviorProblemItems.Commands.Delete;
using BigLion.CPA.Application.Features.BehaviorProblemItems.Commands.Update;
using BigLion.CPA.Application.Features.BehaviorProblemItems.Queries.Get;
using BigLion.CPA.Application.Features.BehaviorProblemItems.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class BehaviorProblemItemsController : BaseController
{
    [HttpPost(Name = "CreateBehaviorProblemItem")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateBehaviorProblemItemCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetBehaviorProblemItem")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BehaviorProblemItemViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<BehaviorProblemItemViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetBehaviorProblemItemQuery { UID = id }));
    }

    [HttpGet(Name = "GetBehaviorProblemItemList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<BehaviorProblemItemViewModel>))]
    public async Task<ActionResult<PaginatedList<BehaviorProblemItemViewModel>>> List([FromQuery] GetBehaviorProblemItemListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateBehaviorProblemItem")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBehaviorProblemItemCommand command)
    {
        if (id != command.UID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteBehaviorProblemItem")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteBehaviorProblemItemCommand { UID = id });
        return NoContent();
    }
}
