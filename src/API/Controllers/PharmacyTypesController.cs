using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.PharmacyTypes.Commands.Create;
using BigLion.CPA.Application.Features.PharmacyTypes.Commands.Delete;
using BigLion.CPA.Application.Features.PharmacyTypes.Commands.Update;
using BigLion.CPA.Application.Features.PharmacyTypes.Queries.Get;
using BigLion.CPA.Application.Features.PharmacyTypes.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class PharmacyTypesController : BaseController
{
    [HttpPost(Name = "CreatePharmacyType")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreatePharmacyTypeCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetPharmacyType")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PharmacyTypeViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<PharmacyTypeViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetPharmacyTypeQuery { Id = id }));
    }

    [HttpGet(Name = "GetPharmacyTypeList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<PharmacyTypeViewModel>))]
    public async Task<ActionResult<PaginatedList<PharmacyTypeViewModel>>> List([FromQuery] GetPharmacyTypeListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdatePharmacyType")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePharmacyTypeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeletePharmacyType")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeletePharmacyTypeCommand { Id = id });
        return NoContent();
    }
}
