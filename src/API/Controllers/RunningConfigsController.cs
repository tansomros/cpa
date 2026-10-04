using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.RunningConfigs.Commands.Create;
using BigLion.CPA.Application.Features.RunningConfigs.Commands.Delete;
using BigLion.CPA.Application.Features.RunningConfigs.Commands.Update;
using BigLion.CPA.Application.Features.RunningConfigs.Queries.Get;
using BigLion.CPA.Application.Features.RunningConfigs.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class RunningConfigsController : BaseController
{
    [HttpPost(Name = "CreateRunningConfig")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateRunningConfigCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id}", Name = "GetRunningConfig")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RunningConfigViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<RunningConfigViewModel>> Get(string id)
    {
        return Ok(await Mediator.Send(new GetRunningConfigQuery { Code = id }));
    }

    [HttpGet(Name = "GetRunningConfigList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<RunningConfigViewModel>))]
    public async Task<ActionResult<PaginatedList<RunningConfigViewModel>>> List([FromQuery] GetRunningConfigListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id}", Name = "UpdateRunningConfig")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateRunningConfigCommand command)
    {
        if (id != command.Code)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteRunningConfig")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new DeleteRunningConfigCommand { Code = id });
        return NoContent();
    }
}
