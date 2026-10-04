using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.ServiceTypes.Commands.Create;
using BigLion.CPA.Application.Features.ServiceTypes.Commands.Delete;
using BigLion.CPA.Application.Features.ServiceTypes.Commands.Update;
using BigLion.CPA.Application.Features.ServiceTypes.Queries.Get;
using BigLion.CPA.Application.Features.ServiceTypes.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class ServiceTypesController : BaseController
{
    [HttpPost(Name = "CreateServiceType")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateServiceTypeCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id}", Name = "GetServiceType")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceTypeViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<ServiceTypeViewModel>> Get(string id)
    {
        return Ok(await Mediator.Send(new GetServiceTypeQuery { ServiceTypeID = id }));
    }

    [HttpGet(Name = "GetServiceTypeList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<ServiceTypeViewModel>))]
    public async Task<ActionResult<PaginatedList<ServiceTypeViewModel>>> List([FromQuery] GetServiceTypeListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id}", Name = "UpdateServiceType")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateServiceTypeCommand command)
    {
        if (id != command.ServiceTypeID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteServiceType")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new DeleteServiceTypeCommand { ServiceTypeID = id });
        return NoContent();
    }
}
