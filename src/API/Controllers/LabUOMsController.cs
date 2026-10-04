using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.LabUOMs.Commands.Create;
using BigLion.CPA.Application.Features.LabUOMs.Commands.Delete;
using BigLion.CPA.Application.Features.LabUOMs.Commands.Update;
using BigLion.CPA.Application.Features.LabUOMs.Queries.Get;
using BigLion.CPA.Application.Features.LabUOMs.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class LabUOMsController : BaseController
{
    [HttpPost(Name = "CreateLabUOM")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateLabUOMCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetLabUOM")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LabUOMViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<LabUOMViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetLabUOMQuery { UID = id }));
    }

    [HttpGet(Name = "GetLabUOMList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<LabUOMViewModel>))]
    public async Task<ActionResult<PaginatedList<LabUOMViewModel>>> List([FromQuery] GetLabUOMListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateLabUOM")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLabUOMCommand command)
    {
        if (id != command.UID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteLabUOM")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteLabUOMCommand { UID = id });
        return NoContent();
    }
}
