using Microsoft.AspNetCore.Mvc;
using Kondongpu.Application.Features.Patients.Commands.Create;
using Kondongpu.Application.Features.Patients.Commands.Update;
using Kondongpu.Application.Features.Patients.Queries.Get;
using Kondongpu.Application.Features.Patients.ViewModels;

namespace Kondongpu.Presentation.API.Controllers
{
    /// <summary>
    /// กลุ่ม API Endpoint สำหรับจัดการข้อมูลผู้เข้ารับบริการ (Patients)
    /// </summary>
    public class PatientsController : BaseController
    {
        /// <summary>
        /// API Endpoint สำหรับเพิ่ม Patient จาก WorkerService
        /// </summary>
        [HttpPost("sync/worker", Name = "CreatePatientFromWorker")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> CreatePatientFromWorker([FromBody] CreatePatientFromWorkerCommand command)
        {
            var id = await Mediator.Send(command);
            return Created($"{Request.Path.Value}/{id}", id);
        }

        /// <summary>
        /// API Endpoint สำหรับดึงข้อมูล Patient จาก Id
        /// </summary>
        [HttpGet("{id}", Name = "GetPatient")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatientViewModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<PatientViewModel>> GetPatient(int id)
        {
            return Ok(await Mediator.Send(new GetPatientQuery { Id = id }));
        }

        /// <summary>
        /// API Endpoint สำหรับดึงข้อมูล Patient จาก HN (Hospital Number)
        /// </summary>
        [HttpGet("hn/{hospitalNumber}", Name = "GetPatientByHN")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatientViewModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<PatientViewModel>> GetPatientByHN(string hospitalNumber)
        {
            return Ok(await Mediator.Send(new GetPatientByHospitalNumberQuery { HospitalNumber = hospitalNumber }));
        }

        /// <summary>
        /// API Endpoint สำหรับอับเดตที่อยู่ Patient
        /// </summary>
        [HttpPut("{id}/address", Name = "UpdatePatientAddress")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpdatePatientAddress(int id, [FromBody] UpdateAdressPatientCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// API Endpoint สำหรับอับเดตที่อยู่ Patient จาก Worker
        /// </summary>
        [HttpPut("hn/{hn}/sync/worker", Name = "UpsertPatientFromWorker")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpsertPatientFromWorker(string hn, [FromBody] UpsertPatientFromWorkerCommand command)
        {
            if (hn != command.HospitalNumber)
            {
                return BadRequest();
            }

            var id = await Mediator.Send(command);
            return Created($"{Request.Path.Value}/{id}", id);
        }
    }
}
