using Microsoft.AspNetCore.Mvc;
using Kondongpu.Application.Features.Lookups;

namespace Kondongpu.Presentation.API.Controllers;

/// <summary>
/// กลุ่ม API Endpoint สำหรับจัดการข้อมูลตัวเลือกพื้นฐาน (Options/Lookups)
/// </summary>
public class OptionsController : BaseController
{
    /// <summary>
    /// ดึงรายการหมวดหมู่ lookup ทั้งหมดที่มีในระบบ
    /// </summary>
    [HttpGet(Name = "GetLookupCategories")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
    public async Task<ActionResult<List<string>>> GetCategories()
    {
        return Ok(await Mediator.Send(new GetLookupCategoriesQuery()));
    }

    /// <summary>
    /// ดึงรายการค่า lookup ตามหมวดหมู่ที่ระบุ
    /// </summary>
    [HttpGet("{category}", Name = "GetLookupOptions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LookupOptionDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<List<LookupOptionDto>>> GetOptions(string category, [FromQuery] string? lang = null)
    {
        return Ok(await Mediator.Send(new GetLookupOptionsQuery { Category = category, Language = lang }));
    }
}
