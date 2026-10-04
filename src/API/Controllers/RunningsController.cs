using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Runnings.Commands.Create;
using BigLion.CPA.Application.Features.Runnings.Commands.Delete;
using BigLion.CPA.Application.Features.Runnings.Commands.Update;
using BigLion.CPA.Application.Features.Runnings.Queries.Get;
using BigLion.CPA.Application.Features.Runnings.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class RunningsController : BaseController
{
    [HttpPost(Name = "CreateRunning")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateRunningCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id}", Name = "GetRunning")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RunningViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<RunningViewModel>> Get(string id)
    {
        return Ok(await Mediator.Send(new GetRunningQuery { Code = id }));
    }

    [HttpGet(Name = "GetRunningList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<RunningViewModel>))]
    public async Task<ActionResult<PaginatedList<RunningViewModel>>> List([FromQuery] GetRunningListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id}", Name = "UpdateRunning")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateRunningCommand command)
    {
        if (id != command.Code)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteRunning")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new DeleteRunningCommand { Code = id });
        return NoContent();
    }
}
