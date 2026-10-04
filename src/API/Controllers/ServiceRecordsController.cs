using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.ServiceRecords.Commands.Create;
using BigLion.CPA.Application.Features.ServiceRecords.Commands.Delete;
using BigLion.CPA.Application.Features.ServiceRecords.Commands.Update;
using BigLion.CPA.Application.Features.ServiceRecords.Queries.Get;
using BigLion.CPA.Application.Features.ServiceRecords.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class ServiceRecordsController : BaseController
{
    [HttpPost(Name = "CreateServices")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(long))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateServicesCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:long}", Name = "GetServices")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServicesViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<ServicesViewModel>> Get(long id)
    {
        return Ok(await Mediator.Send(new GetServicesQuery { itemID = id }));
    }

    [HttpGet(Name = "GetServicesList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<ServicesViewModel>))]
    public async Task<ActionResult<PaginatedList<ServicesViewModel>>> List([FromQuery] GetServicesListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:long}", Name = "UpdateServices")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateServicesCommand command)
    {
        if (id != command.itemID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:long}", Name = "DeleteServices")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(long id)
    {
        await Mediator.Send(new DeleteServicesCommand { itemID = id });
        return NoContent();
    }
}
