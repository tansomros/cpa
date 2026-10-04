using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.UserRoleAssignments.Commands.Create;
using BigLion.CPA.Application.Features.UserRoleAssignments.Commands.Delete;
using BigLion.CPA.Application.Features.UserRoleAssignments.Commands.Update;
using BigLion.CPA.Application.Features.UserRoleAssignments.Queries.Get;
using BigLion.CPA.Application.Features.UserRoleAssignments.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BigLion.CPA.Presentation.API.Controllers;

public class UserRoleAssignmentsController : BaseController
{
    [HttpPost(Name = "CreateUserRoles")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] CreateUserRolesCommand command)
    {
        await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{command.RoleID}/{command.UserID}", null);
    }

    [HttpGet("{roleId:int}/{userId:int}", Name = "GetUserRoles")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserRolesViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<UserRolesViewModel>> Get(int roleId, int userId)
    {
        return Ok(await Mediator.Send(new GetUserRolesQuery { RoleID = roleId, UserID = userId }));
    }

    [HttpGet(Name = "GetUserRolesList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<UserRolesViewModel>))]
    public async Task<ActionResult<PaginatedList<UserRolesViewModel>>> List([FromQuery] GetUserRolesListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPut("{roleId:int}/{userId:int}", Name = "UpdateUserRoles")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(int roleId, int userId, [FromBody] UpdateUserRolesCommand command)
    {
        if (roleId != command.RoleID || userId != command.UserID)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{roleId:int}/{userId:int}", Name = "DeleteUserRoles")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(int roleId, int userId)
    {
        await Mediator.Send(new DeleteUserRolesCommand { RoleID = roleId, UserID = userId });
        return NoContent();
    }
}
