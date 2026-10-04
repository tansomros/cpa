using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.MTMDeseases.Commands.Create;
using BigLion.CPA.Application.Features.MTMDeseases.Commands.Delete;
using BigLion.CPA.Application.Features.MTMDeseases.Commands.Update;
using BigLion.CPA.Application.Features.MTMDeseases.Queries.Get;
using BigLion.CPA.Application.Features.MTMDeseases.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class MTMDeseasesController : BaseController
{
    [HttpPost(Name = "CreateMTMDesease")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateMTMDeseaseCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetMTMDesease")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MTMDeseaseViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<MTMDeseaseViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetMTMDeseaseQuery { UID = id }));
    }

    [HttpGet(Name = "GetMTMDeseaseList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<MTMDeseaseViewModel>))]
    public async Task<ActionResult<PaginatedList<MTMDeseaseViewModel>>> List([FromQuery] GetMTMDeseaseListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateMTMDesease")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMTMDeseaseCommand command)
    {
        if (id != command.UID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteMTMDesease")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteMTMDeseaseCommand { UID = id });
        return NoContent();
    }
}
