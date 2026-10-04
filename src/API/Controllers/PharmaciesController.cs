using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Pharmacy.Commands.Delete;
using BigLion.CPA.Application.Features.Pharmacy.Queries.Get;
using BigLion.CPA.Application.Features.Pharmacy.Commands.Create;
using BigLion.CPA.Application.Features.Pharmacy.Commands.Update;
using BigLion.CPA.Application.Features.Pharmacy.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class PharmaciesController : BaseController
{
    [HttpPost(Name = "CreatePharmacy")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreatePharmacy([FromBody] CreatePharmacyCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpPut("{id}", Name = "UpdatePharmacy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdatePharmacy(int id, [FromBody] UpdatePharmacyCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeletePharmacy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeletePharmacy(int id, [FromQuery] uint rowVersion)
    {
        await Mediator.Send(new DeletePharmacyCommand { Id = id, RowVersion = rowVersion });
        return NoContent();
    }

    [HttpGet("lookups", Name = "GetPharmacyLookups")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PharmacyLookupsViewModel))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PharmacyLookupsViewModel>> GetPharmacyLookups([FromQuery] GetPharmacyLookupsQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpGet("{id:int}", Name = "GetPharmacy")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PharmacyViewModel))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PharmacyViewModel>> GetPharmacy(int id)
    {
        return base.Ok(await Mediator.Send(new Application.Features.Pharmacy.Queries.Get.GetPharmacyQuery { Id = id }));
    }

    [HttpGet(Name = "GetPharmacies")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<PharmacyViewModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PaginatedList<PharmacyViewModel>>> GetPharmacies([FromQuery] Application.Features.Pharmacy.Queries.Get.GetPharmaciesQuery query)
    {
        return Ok(await Mediator.Send(query));
    }
}
