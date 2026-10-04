using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Dispenses.Commands.Create;
using BigLion.CPA.Application.Features.Dispenses.Commands.Delete;
using BigLion.CPA.Application.Features.Dispenses.Commands.Update;
using BigLion.CPA.Application.Features.Dispenses.Queries.Get;
using BigLion.CPA.Application.Features.Dispenses.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class DispensesController : BaseController
{
    [HttpPost(Name = "CreateDispense")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(long))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateDispenseCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:long}", Name = "GetDispense")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DispenseViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<DispenseViewModel>> Get(long id)
    {
        return Ok(await Mediator.Send(new GetDispenseQuery { UID = id }));
    }

    [HttpGet(Name = "GetDispenseList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<DispenseViewModel>))]
    public async Task<ActionResult<PaginatedList<DispenseViewModel>>> List([FromQuery] GetDispenseListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:long}", Name = "UpdateDispense")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateDispenseCommand command)
    {
        if (id != command.UID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:long}", Name = "DeleteDispense")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(long id)
    {
        await Mediator.Send(new DeleteDispenseCommand { UID = id });
        return NoContent();
    }
}
