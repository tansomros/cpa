using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.PaymentConfigs.Commands.Create;
using BigLion.CPA.Application.Features.PaymentConfigs.Commands.Delete;
using BigLion.CPA.Application.Features.PaymentConfigs.Commands.Update;
using BigLion.CPA.Application.Features.PaymentConfigs.Queries.Get;
using BigLion.CPA.Application.Features.PaymentConfigs.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class PaymentConfigsController : BaseController
{
    [HttpPost(Name = "CreatePaymentConfig")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreatePaymentConfigCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetPaymentConfig")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentConfigViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<PaymentConfigViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetPaymentConfigQuery { itemID = id }));
    }

    [HttpGet(Name = "GetPaymentConfigList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<PaymentConfigViewModel>))]
    public async Task<ActionResult<PaginatedList<PaymentConfigViewModel>>> List([FromQuery] GetPaymentConfigListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdatePaymentConfig")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymentConfigCommand command)
    {
        if (id != command.itemID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeletePaymentConfig")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeletePaymentConfigCommand { itemID = id });
        return NoContent();
    }
}
