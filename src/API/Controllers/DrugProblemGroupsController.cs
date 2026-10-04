using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.DrugProblemGroups.Commands.Create;
using BigLion.CPA.Application.Features.DrugProblemGroups.Commands.Delete;
using BigLion.CPA.Application.Features.DrugProblemGroups.Commands.Update;
using BigLion.CPA.Application.Features.DrugProblemGroups.Queries.Get;
using BigLion.CPA.Application.Features.DrugProblemGroups.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class DrugProblemGroupsController : BaseController
{
    [HttpPost(Name = "CreateDrugProblemGroup")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateDrugProblemGroupCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id}", Name = "GetDrugProblemGroup")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DrugProblemGroupViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<DrugProblemGroupViewModel>> Get(string id)
    {
        return Ok(await Mediator.Send(new GetDrugProblemGroupQuery { Code = id }));
    }

    [HttpGet(Name = "GetDrugProblemGroupList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<DrugProblemGroupViewModel>))]
    public async Task<ActionResult<PaginatedList<DrugProblemGroupViewModel>>> List([FromQuery] GetDrugProblemGroupListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id}", Name = "UpdateDrugProblemGroup")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateDrugProblemGroupCommand command)
    {
        if (id != command.Code)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteDrugProblemGroup")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new DeleteDrugProblemGroupCommand { Code = id });
        return NoContent();
    }
}
