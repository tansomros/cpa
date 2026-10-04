using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.SubDistricts.Commands.Create;
using BigLion.CPA.Application.Features.SubDistricts.Commands.Delete;
using BigLion.CPA.Application.Features.SubDistricts.Commands.Update;
using BigLion.CPA.Application.Features.SubDistricts.Queries.Get;
using BigLion.CPA.Application.Features.SubDistricts.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class SubDistrictsController : BaseController
{
    [HttpPost(Name = "CreateSubDistrict")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateSubDistrictCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id}", Name = "GetSubDistrict")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubDistrictViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<SubDistrictViewModel>> Get(string id)
    {
        return Ok(await Mediator.Send(new GetSubDistrictQuery { SubDistrictId = id }));
    }

    [HttpGet(Name = "GetSubDistrictList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<SubDistrictViewModel>))]
    public async Task<ActionResult<PaginatedList<SubDistrictViewModel>>> List([FromQuery] GetSubDistrictListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id}", Name = "UpdateSubDistrict")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateSubDistrictCommand command)
    {
        if (id != command.SubDistrictId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteSubDistrict")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new DeleteSubDistrictCommand { SubDistrictId = id });
        return NoContent();
    }
}
