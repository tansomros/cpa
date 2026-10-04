using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Patients.Commands.Create;
using BigLion.CPA.Application.Features.Patients.Commands.Delete;
using BigLion.CPA.Application.Features.Patients.Commands.Update;
using BigLion.CPA.Application.Features.Patients.Queries.Get;
using BigLion.CPA.Application.Features.Patients.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class PatientsController : BaseController
{
    [HttpPost(Name = "CreatePatient")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreatePatientCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetPatient")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatientViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<PatientViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetPatientQuery { Id = id }));
    }

    [HttpGet(Name = "GetPatientList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<PatientViewModel>))]
    public async Task<ActionResult<PaginatedList<PatientViewModel>>> List([FromQuery] GetPatientListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdatePatient")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeletePatient")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeletePatientCommand { Id = id });
        return NoContent();
    }
}
