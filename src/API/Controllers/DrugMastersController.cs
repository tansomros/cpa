using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.DrugMasters.Commands.Create;
using BigLion.CPA.Application.Features.DrugMasters.Commands.Delete;
using BigLion.CPA.Application.Features.DrugMasters.Commands.Update;
using BigLion.CPA.Application.Features.DrugMasters.Queries.Get;
using BigLion.CPA.Application.Features.DrugMasters.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class DrugMastersController : BaseController
{
    [HttpPost(Name = "CreateDrugMaster")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateDrugMasterCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetDrugMaster")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DrugMasterViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<DrugMasterViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetDrugMasterQuery { UID = id }));
    }

    [HttpGet(Name = "GetDrugMasterList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<DrugMasterViewModel>))]
    public async Task<ActionResult<PaginatedList<DrugMasterViewModel>>> List([FromQuery] GetDrugMasterListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateDrugMaster")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDrugMasterCommand command)
    {
        if (id != command.UID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteDrugMaster")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteDrugMasterCommand { UID = id });
        return NoContent();
    }
}
