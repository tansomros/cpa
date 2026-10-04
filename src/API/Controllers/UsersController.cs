using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.Users.Commands.Create;
using BigLion.CPA.Application.Features.Users.Commands.Delete;
using BigLion.CPA.Application.Features.Users.Commands.Update;
using BigLion.CPA.Application.Features.Users.Queries.Get;
using BigLion.CPA.Application.Features.Users.ViewModel;
using BigLion.CPA.Application.Identity.Commands;

namespace BigLion.CPA.Presentation.API.Controllers;

/// <summary>
/// กลุ่ม API Endpoint สำหรับจัดการข้อมูลการตรวจเอกซเรย์ (X-Rays)
/// </summary>
public class UsersController : BaseController
{
    /// <summary>
    /// API Endpoint สำหรับเพิ่ม User
    /// </summary>
    [HttpPost(Name = "CreateUser")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var id = await Mediator.Send(command);
        return Created($"{Request.Path.Value}/{id}", id);
    }

    /// <summary>
    /// API Endpoint สำหรับอับเดต User
    /// </summary>
    [HttpPut("{id}", Name = "UpdateUser")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }
        

    /// <summary>
    /// API Endpoint สำหรับลบข้อมูลUserด้วย Id
    /// </summary>
    [HttpDelete("{id}", Name = "DeleteUser")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await Mediator.Send(new DeleteUserCommand { Id = id });
        return NoContent();
    }

    /// <summary>
    /// API Endpoint สำหรับดึงข้อมูล User จาก id
    /// </summary>
    [HttpGet("{id:int}", Name = "GetUser")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<UserViewModel>> GetUser(int id)
    {
        return Ok(await Mediator.Send(new GetUserQuery { Id = id }));
    }

    /// <summary>
    /// API Endpoint สำหรับดึงข้อมูลรายการ User
    /// </summary>
    [HttpGet(Name = "GetUserList")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<UserViewModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<PaginatedList<UserViewModel>>> GetUserList([FromQuery] GetUserListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

   
    /// <summary>
    /// API Endpoint สำหรับ Login
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login", Name = "Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserViewModel))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(command, cancellationToken);
        return Ok(response);
    }
        
}
