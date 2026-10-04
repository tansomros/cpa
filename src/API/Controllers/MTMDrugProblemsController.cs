using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.MTMDrugProblems.Commands.Create;
using BigLion.CPA.Application.Features.MTMDrugProblems.Commands.Delete;
using BigLion.CPA.Application.Features.MTMDrugProblems.Commands.Update;
using BigLion.CPA.Application.Features.MTMDrugProblems.Queries.Get;
using BigLion.CPA.Application.Features.MTMDrugProblems.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class MTMDrugProblemsController : BaseController
{
    [HttpPost(Name = "CreateMTMDrugProblem")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateMTMDrugProblemCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetMTMDrugProblem")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MTMDrugProblemViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<MTMDrugProblemViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetMTMDrugProblemQuery { UID = id }));
    }

    [HttpGet(Name = "GetMTMDrugProblemList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<MTMDrugProblemViewModel>))]
    public async Task<ActionResult<PaginatedList<MTMDrugProblemViewModel>>> List([FromQuery] GetMTMDrugProblemListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateMTMDrugProblem")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMTMDrugProblemCommand command)
    {
        if (id != command.UID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteMTMDrugProblem")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteMTMDrugProblemCommand { UID = id });
        return NoContent();
    }
}
