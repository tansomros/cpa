using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Prefixs.Commands.Create;
using BigLion.CPA.Application.Features.Prefixs.Commands.Delete;
using BigLion.CPA.Application.Features.Prefixs.Commands.Update;
using BigLion.CPA.Application.Features.Prefixs.Queries.Get;
using BigLion.CPA.Application.Features.Prefixs.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class PrefixsController : BaseController
{
    [HttpPost(Name = "CreatePrefix")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreatePrefixCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetPrefix")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PrefixViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<PrefixViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetPrefixQuery { Id = id }));
    }

    [HttpGet(Name = "GetPrefixList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<PrefixViewModel>))]
    public async Task<ActionResult<PaginatedList<PrefixViewModel>>> List([FromQuery] GetPrefixListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdatePrefix")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePrefixCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeletePrefix")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeletePrefixCommand { Id = id });
        return NoContent();
    }
}
