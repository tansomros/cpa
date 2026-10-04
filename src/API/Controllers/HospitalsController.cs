using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Hospitals.Commands.Create;
using BigLion.CPA.Application.Features.Hospitals.Commands.Delete;
using BigLion.CPA.Application.Features.Hospitals.Commands.Update;
using BigLion.CPA.Application.Features.Hospitals.Queries.Get;
using BigLion.CPA.Application.Features.Hospitals.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class HospitalsController : BaseController
{
    [HttpPost(Name = "CreateHospital")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateHospitalCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetHospital")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HospitalViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<HospitalViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetHospitalQuery { HospitalUID = id }));
    }

    [HttpGet(Name = "GetHospitalList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<HospitalViewModel>))]
    public async Task<ActionResult<PaginatedList<HospitalViewModel>>> List([FromQuery] GetHospitalListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateHospital")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateHospitalCommand command)
    {
        if (id != command.HospitalUID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteHospital")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteHospitalCommand { HospitalUID = id });
        return NoContent();
    }
}
