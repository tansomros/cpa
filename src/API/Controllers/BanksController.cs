using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Banks.Commands.Create;
using BigLion.CPA.Application.Features.Banks.Commands.Delete;
using BigLion.CPA.Application.Features.Banks.Commands.Update;
using BigLion.CPA.Application.Features.Banks.Queries.Get;
using BigLion.CPA.Application.Features.Banks.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class BanksController : BaseController
{
    [HttpPost(Name = "CreateBank")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateBankCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetBank")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BankViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<BankViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetBankQuery { Id = id }));
    }

    [HttpGet(Name = "GetBankList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<BankViewModel>))]
    public async Task<ActionResult<PaginatedList<BankViewModel>>> List([FromQuery] GetBankListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateBank")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBankCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteBank")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteBankCommand { Id = id });
        return NoContent();
    }
}
