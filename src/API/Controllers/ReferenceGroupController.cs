using Microsoft.AspNetCore.Mvc;
using Kondongpu.Application.Features.ReferenceGroups.Commands.Create;
using Kondongpu.Application.Features.ReferenceGroups.Commands.Delete;
using Kondongpu.Application.Features.ReferenceGroups.Commands.Update;
using Kondongpu.Application.Features.ReferenceGroups.Queries.Get;
using Kondongpu.Application.Features.ReferenceGroups.ViewModels;

namespace Kondongpu.Presentation.API.Controllers;

/// <summary>
/// กลุ่ม API Endpoint สำหรับจัดการข้อมูลกลุ่มค่าอ้างอิง (Reference Groups)
/// </summary>
[Obsolete("ใช้ OptionsController แทน — GET /Options/{category}")]
public class ReferenceGroupController : BaseController
{
    [HttpPost(Name = "CreateReferenceGroup")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> CreateReferenceGroup([FromBody] CreateReferenceGroupCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpPut("{id}", Name = "UpdateReferenceGroup")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateReferenceGroup(int id, [FromBody] UpdateReferenceGroupCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteReferenceGroup")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteReferenceGroup(int id)
    {
        await Mediator.Send(new DeleteReferenceGroupCommand { Id = id });
        return NoContent();
    }

    [HttpGet("{id}", Name = "GetReferenceGroup")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReferenceGroupViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<ReferenceGroupViewModel>> GetReferenceGroup(int id)
    {
        return Ok(await Mediator.Send(new GetReferenceGroupQuery { Id = id }));
    }
}
