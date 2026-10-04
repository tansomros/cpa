using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.MTMDrugRemains.Commands.Create;
using BigLion.CPA.Application.Features.MTMDrugRemains.Commands.Delete;
using BigLion.CPA.Application.Features.MTMDrugRemains.Commands.Update;
using BigLion.CPA.Application.Features.MTMDrugRemains.Queries.Get;
using BigLion.CPA.Application.Features.MTMDrugRemains.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class MTMDrugRemainsController : BaseController
{
    [HttpPost(Name = "CreateMTMDrugRemain")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateMTMDrugRemainCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetMTMDrugRemain")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MTMDrugRemainViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<MTMDrugRemainViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetMTMDrugRemainQuery { UID = id }));
    }

    [HttpGet(Name = "GetMTMDrugRemainList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<MTMDrugRemainViewModel>))]
    public async Task<ActionResult<PaginatedList<MTMDrugRemainViewModel>>> List([FromQuery] GetMTMDrugRemainListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateMTMDrugRemain")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMTMDrugRemainCommand command)
    {
        if (id != command.UID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteMTMDrugRemain")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteMTMDrugRemainCommand { UID = id });
        return NoContent();
    }
}
