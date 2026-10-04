using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.MTMs.Commands.Create;
using BigLion.CPA.Application.Features.MTMs.Commands.Delete;
using BigLion.CPA.Application.Features.MTMs.Commands.Update;
using BigLion.CPA.Application.Features.MTMs.Queries.Get;
using BigLion.CPA.Application.Features.MTMs.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class MTMsController : BaseController
{
    [HttpPost(Name = "CreateMTM")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateMTMCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetMTM")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MTMViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<MTMViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetMTMQuery { UID = id }));
    }

    [HttpGet(Name = "GetMTMList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<MTMViewModel>))]
    public async Task<ActionResult<PaginatedList<MTMViewModel>>> List([FromQuery] GetMTMListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateMTM")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMTMCommand command)
    {
        if (id != command.UID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteMTM")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteMTMCommand { UID = id });
        return NoContent();
    }
}
