using Microsoft.AspNetCore.Mvc;
using Kondongpu.Application.Features.Addresses.Queries.Get;
using Kondongpu.Application.Features.Addresses.ViewModel;

namespace Kondongpu.Presentation.API.Controllers
{
    /// <summary>
    /// กลุ่ม API Endpoint สำหรับดึงข้อมูลจังหวัด อำเภอ ตำบล ไทย
    /// </summary>
    public class AddressesController : BaseController
    {
        /// <summary>
        /// ดึงข้อมูลจังหวัด
        /// </summary>
        [HttpGet("provinces", Name = "GetProvinces")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProvinceListViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ProvinceListViewModel>> GetProvinces()
        {
            return Ok(await Mediator.Send(new GetProvinceListQuery()));
        }

        /// <summary>
        /// ดึงข้อมูลอำเภอด้วย Id ของจังหวัด
        /// </summary>
        [HttpGet("provinces/{provinceId}/districts", Name = "GetDistricts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DistrictListViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<DistrictListViewModel>> GetDistricts(string provinceId)
        {
            return Ok(await Mediator.Send(new GetDistrictListQuery { ProvinceId = provinceId }));
        }

        /// <summary>
        /// ดึงข้อมูลตำบลด้วย Id ของอำเภอ
        /// </summary>
        [HttpGet("districts/{districtId}/sub-districts", Name = "GetSubDistricts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubDistrictListViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<SubDistrictListViewModel>> GetSubDistricts(string districtId)
        {
            return Ok(await Mediator.Send(new GetSubDistrictListQuery { DistrictId = districtId }));
        }
    }
}
