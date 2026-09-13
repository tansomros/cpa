using Microsoft.AspNetCore.Mvc;
using Kondongpu.Application.Features.ReferenceValues.Commands.Create;
using Kondongpu.Application.Features.ReferenceValues.Commands.Delete;
using Kondongpu.Application.Features.ReferenceValues.Commands.Update;
using Kondongpu.Application.Features.ReferenceValues.Queries.Get;
using Kondongpu.Application.Features.ReferenceValues.ViewModels;

namespace Kondongpu.Presentation.API.Controllers;

/// <summary>
/// กลุ่ม API Endpoint สำหรับจัดการข้อมูลค่าอ้างอิง (Reference Values)
/// </summary>
[Obsolete("ใช้ OptionsController แทน — GET /Options/{category}")]
public class ReferenceValueController : BaseController
{
    [HttpPost(Name = "CreateReferenceValue")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> CreateReferenceValue([FromBody] CreateReferenceValueCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpPut("{id}", Name = "UpdateReferenceValue")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateReferenceValue(int id, [FromBody] UpdateReferenceValueCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteReferenceValue")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteReferenceValue(int id)
    {
        await Mediator.Send(new DeleteReferenceValueCommand { Id = id });
        return NoContent();
    }

    [HttpGet("{id}", Name = "GetReferenceValue")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReferenceValueViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<ReferenceValueViewModel>> GetReferenceValue(int id)
    {
        return Ok(await Mediator.Send(new GetReferenceValueQuery { Id = id }));
    }

    [HttpGet("groups/{groupId}", Name = "GetReferenceValueByGroup")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReferenceValueListViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<ReferenceValueListViewModel>> GetReferenceValueByGroup(int groupId)
    {
        return Ok(await Mediator.Send(new GetReferenceValueByGroupQuery { ReferenceGroupId = groupId }));
    }

    [HttpGet("groups/{groupId}/abnormal", Name = "GetReferenceValueAbnormalByGroup")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReferenceValueAbnormalListViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<ReferenceValueAbnormalListViewModel>> GetReferenceValueAbnormalByGroup(int groupId)
    {
        return Ok(await Mediator.Send(new GetReferenceValueAbnormalByGroupQuery { ReferenceGroupId = groupId }));
    }
}
