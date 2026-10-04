using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.DrugProblemItems.Commands.Create;
using BigLion.CPA.Application.Features.DrugProblemItems.Commands.Delete;
using BigLion.CPA.Application.Features.DrugProblemItems.Commands.Update;
using BigLion.CPA.Application.Features.DrugProblemItems.Queries.Get;
using BigLion.CPA.Application.Features.DrugProblemItems.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class DrugProblemItemsController : BaseController
{
    [HttpPost(Name = "CreateDrugProblemItem")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateDrugProblemItemCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id}", Name = "GetDrugProblemItem")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DrugProblemItemViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<DrugProblemItemViewModel>> Get(string id)
    {
        return Ok(await Mediator.Send(new GetDrugProblemItemQuery { Code = id }));
    }

    [HttpGet(Name = "GetDrugProblemItemList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<DrugProblemItemViewModel>))]
    public async Task<ActionResult<PaginatedList<DrugProblemItemViewModel>>> List([FromQuery] GetDrugProblemItemListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id}", Name = "UpdateDrugProblemItem")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateDrugProblemItemCommand command)
    {
        if (id != command.Code)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteDrugProblemItem")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new DeleteDrugProblemItemCommand { Code = id });
        return NoContent();
    }
}
