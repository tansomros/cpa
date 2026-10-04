using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Provinces.Commands.Create;
using BigLion.CPA.Application.Features.Provinces.Commands.Delete;
using BigLion.CPA.Application.Features.Provinces.Commands.Update;
using BigLion.CPA.Application.Features.Provinces.Queries.Get;
using BigLion.CPA.Application.Features.Provinces.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class ProvincesController : BaseController
{
    [HttpPost(Name = "CreateProvince")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateProvinceCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id}", Name = "GetProvince")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProvinceViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<ProvinceViewModel>> Get(string id)
    {
        return Ok(await Mediator.Send(new GetProvinceQuery { Id = id }));
    }

    [HttpGet(Name = "GetProvinceList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<ProvinceViewModel>))]
    public async Task<ActionResult<PaginatedList<ProvinceViewModel>>> List([FromQuery] GetProvinceListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id}", Name = "UpdateProvince")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProvinceCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteProvince")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new DeleteProvinceCommand { Id = id });
        return NoContent();
    }
}
