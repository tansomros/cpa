using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.LabResults.Commands.Create;
using BigLion.CPA.Application.Features.LabResults.Commands.Delete;
using BigLion.CPA.Application.Features.LabResults.Commands.Update;
using BigLion.CPA.Application.Features.LabResults.Queries.Get;
using BigLion.CPA.Application.Features.LabResults.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class LabResultsController : BaseController
{
    [HttpPost(Name = "CreateLabResult")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateLabResultCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetLabResult")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LabResultViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<LabResultViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetLabResultQuery { UID = id }));
    }

    [HttpGet(Name = "GetLabResultList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<LabResultViewModel>))]
    public async Task<ActionResult<PaginatedList<LabResultViewModel>>> List([FromQuery] GetLabResultListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateLabResult")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLabResultCommand command)
    {
        if (id != command.UID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteLabResult")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteLabResultCommand { UID = id });
        return NoContent();
    }
}
