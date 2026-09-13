using Microsoft.AspNetCore.Mvc;
using Cpa.Application.Common.Models;
using Cpa.Application.Features.Users.Commands.Create;
using Cpa.Application.Features.Users.Commands.Delete;
using Cpa.Application.Features.Users.Commands.Update;
using Cpa.Application.Features.Users.Queries.Get;
using Cpa.Application.Features.Users.ViewModel;

namespace Cpa.Presentation.API.Controllers;

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
    [HttpGet("visits/{visitNumber}", Name = "GetUser")]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserListViewModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<UserListViewModel>> GetUserList([FromQuery] GetUserListQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

   
    /// <summary>
    /// API Endpoint สำหรับ Login 
    /// </summary>
    [HttpGet("login", Name = "GetUserLogin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserListViewModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<UserListViewModel>> GetUserLogin(string username,string password)
    {
        var query = new GetUserLoginQuery { Username = username,Password=password };
        return Ok(await Mediator.Send(query));
    }
        
}
