using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.ProvinceGroups.Commands.Create;
using BigLion.CPA.Application.Features.ProvinceGroups.Commands.Delete;
using BigLion.CPA.Application.Features.ProvinceGroups.Commands.Update;
using BigLion.CPA.Application.Features.ProvinceGroups.Queries.Get;
using BigLion.CPA.Application.Features.ProvinceGroups.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class ProvinceGroupsController : BaseController
{
    [HttpPost(Name = "CreateProvinceGroup")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateProvinceGroupCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id}", Name = "GetProvinceGroup")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProvinceGroupViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<ProvinceGroupViewModel>> Get(string id)
    {
        return Ok(await Mediator.Send(new GetProvinceGroupQuery { Id = id }));
    }

    [HttpGet(Name = "GetProvinceGroupList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<ProvinceGroupViewModel>))]
    public async Task<ActionResult<PaginatedList<ProvinceGroupViewModel>>> List([FromQuery] GetProvinceGroupListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id}", Name = "UpdateProvinceGroup")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProvinceGroupCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteProvinceGroup")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new DeleteProvinceGroupCommand { Id = id });
        return NoContent();
    }
}
