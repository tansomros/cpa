using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Districts.Commands.Create;
using BigLion.CPA.Application.Features.Districts.Commands.Delete;
using BigLion.CPA.Application.Features.Districts.Commands.Update;
using BigLion.CPA.Application.Features.Districts.Queries.Get;
using BigLion.CPA.Application.Features.Districts.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class DistrictsController : BaseController
{
    [HttpPost(Name = "CreateDistrict")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateDistrictCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id}", Name = "GetDistrict")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DistrictViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<DistrictViewModel>> Get(string id)
    {
        return Ok(await Mediator.Send(new GetDistrictQuery { Id = id }));
    }

    [HttpGet(Name = "GetDistrictList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<DistrictViewModel>))]
    public async Task<ActionResult<PaginatedList<DistrictViewModel>>> List([FromQuery] GetDistrictListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id}", Name = "UpdateDistrict")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateDistrictCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteDistrict")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new DeleteDistrictCommand { Id = id });
        return NoContent();
    }
}
