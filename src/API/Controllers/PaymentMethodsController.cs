using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.PaymentMethods.Commands.Create;
using BigLion.CPA.Application.Features.PaymentMethods.Commands.Delete;
using BigLion.CPA.Application.Features.PaymentMethods.Commands.Update;
using BigLion.CPA.Application.Features.PaymentMethods.Queries.Get;
using BigLion.CPA.Application.Features.PaymentMethods.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class PaymentMethodsController : BaseController
{
    [HttpPost(Name = "CreatePaymentMethod")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreatePaymentMethodCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetPaymentMethod")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentMethodViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<PaymentMethodViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetPaymentMethodQuery { PaymentID = id }));
    }

    [HttpGet(Name = "GetPaymentMethodList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<PaymentMethodViewModel>))]
    public async Task<ActionResult<PaginatedList<PaymentMethodViewModel>>> List([FromQuery] GetPaymentMethodListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdatePaymentMethod")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymentMethodCommand command)
    {
        if (id != command.PaymentID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeletePaymentMethod")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeletePaymentMethodCommand { PaymentID = id });
        return NoContent();
    }
}
