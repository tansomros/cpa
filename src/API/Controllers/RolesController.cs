using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Roles.Commands.Create;
using BigLion.CPA.Application.Features.Roles.Commands.Delete;
using BigLion.CPA.Application.Features.Roles.Commands.Update;
using BigLion.CPA.Application.Features.Roles.Queries.Get;
using BigLion.CPA.Application.Features.Roles.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class RolesController : BaseController
{
    [HttpPost(Name = "CreateRole")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    [HttpGet("{id:int}", Name = "GetRole")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<RoleViewModel>> Get(int id)
    {
        return Ok(await Mediator.Send(new GetRoleQuery { Id = id }));
    }

    [HttpGet(Name = "GetRoleList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<RoleViewModel>))]
    public async Task<ActionResult<PaginatedList<RoleViewModel>>> List([FromQuery] GetRoleListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{id:int}", Name = "UpdateRole")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteRole")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteRoleCommand { Id = id });
        return NoContent();
    }
}
