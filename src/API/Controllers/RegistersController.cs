using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Registers.Commands.Create;
using BigLion.CPA.Application.Features.Registers.Commands.Delete;
using BigLion.CPA.Application.Features.Registers.Commands.Update;
using BigLion.CPA.Application.Features.Registers.Queries.Get;
using BigLion.CPA.Application.Features.Registers.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class RegistersController : BaseController
{
    [HttpPost(Name = "CreateRegister")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateRegisterCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetRegister")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<RegisterViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetRegisterQuery { UID = id }));
    }

    [HttpGet(Name = "GetRegisterList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<RegisterViewModel>))]
    public async Task<ActionResult<PaginatedList<RegisterViewModel>>> List([FromQuery] GetRegisterListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateRegister")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRegisterCommand command)
    {
        if (id != command.UID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteRegister")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteRegisterCommand { UID = id });
        return NoContent();
    }
}
