using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.LabItems.Commands.Create;
using BigLion.CPA.Application.Features.LabItems.Commands.Delete;
using BigLion.CPA.Application.Features.LabItems.Commands.Update;
using BigLion.CPA.Application.Features.LabItems.Queries.Get;
using BigLion.CPA.Application.Features.LabItems.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class LabItemsController : BaseController
{
    [HttpPost(Name = "CreateLabItem")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateLabItemCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetLabItem")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LabItemViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<LabItemViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetLabItemQuery { UID = id }));
    }

    [HttpGet(Name = "GetLabItemList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<LabItemViewModel>))]
    public async Task<ActionResult<PaginatedList<LabItemViewModel>>> List([FromQuery] GetLabItemListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateLabItem")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLabItemCommand command)
    {
        if (id != command.UID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteLabItem")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteLabItemCommand { UID = id });
        return NoContent();
    }
}
