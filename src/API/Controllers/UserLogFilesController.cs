using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.UserLogFiles.Commands.Create;
using BigLion.CPA.Application.Features.UserLogFiles.Commands.Delete;
using BigLion.CPA.Application.Features.UserLogFiles.Commands.Update;
using BigLion.CPA.Application.Features.UserLogFiles.Queries.Get;
using BigLion.CPA.Application.Features.UserLogFiles.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class UserLogFilesController : BaseController
{
    [HttpPost(Name = "CreateUserLogFile")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(long))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateUserLogFileCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:long}", Name = "GetUserLogFile")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserLogFileViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<UserLogFileViewModel>> Get(long id)
    {
        return Ok(await Mediator.Send(new GetUserLogFileQuery { LogID = id }));
    }

    [HttpGet(Name = "GetUserLogFileList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<UserLogFileViewModel>))]
    public async Task<ActionResult<PaginatedList<UserLogFileViewModel>>> List([FromQuery] GetUserLogFileListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:long}", Name = "UpdateUserLogFile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateUserLogFileCommand command)
    {
        if (id != command.LogID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:long}", Name = "DeleteUserLogFile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(long id)
    {
        await Mediator.Send(new DeleteUserLogFileCommand { LogID = id });
        return NoContent();
    }
}
