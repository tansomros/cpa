using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Pharmacists.Commands.Create;
using BigLion.CPA.Application.Features.Pharmacists.Commands.Delete;
using BigLion.CPA.Application.Features.Pharmacists.Commands.Update;
using BigLion.CPA.Application.Features.Pharmacists.Queries.Get;
using BigLion.CPA.Application.Features.Pharmacists.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class PharmacistsController : BaseController
{
    [HttpPost(Name = "CreatePharmacist")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreatePharmacistCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetPharmacist")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PharmacistViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<PharmacistViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetPharmacistQuery { Id = id }));
    }

    [HttpGet(Name = "GetPharmacistList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<PharmacistViewModel>))]
    public async Task<ActionResult<PaginatedList<PharmacistViewModel>>> List([FromQuery] GetPharmacistListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdatePharmacist")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePharmacistCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeletePharmacist")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeletePharmacistCommand { Id = id });
        return NoContent();
    }
}
