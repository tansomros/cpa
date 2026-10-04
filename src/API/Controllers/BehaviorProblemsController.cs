using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.BehaviorProblems.Commands.Create;
using BigLion.CPA.Application.Features.BehaviorProblems.Commands.Delete;
using BigLion.CPA.Application.Features.BehaviorProblems.Commands.Update;
using BigLion.CPA.Application.Features.BehaviorProblems.Queries.Get;
using BigLion.CPA.Application.Features.BehaviorProblems.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class BehaviorProblemsController : BaseController
{
    [HttpPost(Name = "CreateBehaviorProblem")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateBehaviorProblemCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetBehaviorProblem")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BehaviorProblemViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<BehaviorProblemViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetBehaviorProblemQuery { UID = id }));
    }

    [HttpGet(Name = "GetBehaviorProblemList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<BehaviorProblemViewModel>))]
    public async Task<ActionResult<PaginatedList<BehaviorProblemViewModel>>> List([FromQuery] GetBehaviorProblemListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateBehaviorProblem")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBehaviorProblemCommand command)
    {
        if (id != command.UID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteBehaviorProblem")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteBehaviorProblemCommand { UID = id });
        return NoContent();
    }
}
