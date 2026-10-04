using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.MTMRefers.Commands.Create;
using BigLion.CPA.Application.Features.MTMRefers.Commands.Delete;
using BigLion.CPA.Application.Features.MTMRefers.Commands.Update;
using BigLion.CPA.Application.Features.MTMRefers.Queries.Get;
using BigLion.CPA.Application.Features.MTMRefers.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class MTMRefersController : BaseController
{
    [HttpPost(Name = "CreateMTMRefer")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateMTMReferCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetMTMRefer")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MTMReferViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<MTMReferViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetMTMReferQuery { UID = id }));
    }

    [HttpGet(Name = "GetMTMReferList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<MTMReferViewModel>))]
    public async Task<ActionResult<PaginatedList<MTMReferViewModel>>> List([FromQuery] GetMTMReferListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateMTMRefer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMTMReferCommand command)
    {
        if (id != command.UID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteMTMRefer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteMTMReferCommand { UID = id });
        return NoContent();
    }
}
