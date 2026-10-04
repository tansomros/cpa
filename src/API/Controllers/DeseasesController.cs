using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Deseases.Commands.Create;
using BigLion.CPA.Application.Features.Deseases.Commands.Delete;
using BigLion.CPA.Application.Features.Deseases.Commands.Update;
using BigLion.CPA.Application.Features.Deseases.Queries.Get;
using BigLion.CPA.Application.Features.Deseases.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class DeseasesController : BaseController
{
    [HttpPost(Name = "CreateDesease")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateDeseaseCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetDesease")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeseaseViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<DeseaseViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetDeseaseQuery { UID = id }));
    }

    [HttpGet(Name = "GetDeseaseList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<DeseaseViewModel>))]
    public async Task<ActionResult<PaginatedList<DeseaseViewModel>>> List([FromQuery] GetDeseaseListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateDesease")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDeseaseCommand command)
    {
        if (id != command.UID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteDesease")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteDeseaseCommand { UID = id });
        return NoContent();
    }
}
