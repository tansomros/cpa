using Cpa.Application.Common.Interfaces;
using Cpa.Application.Exceptions;
using Cpa.Domain.Entities;

namespace Cpa.Application.Features.Patients.Commands.Update;

public record UpdateAdressPatientCommand : IRequest<Unit>
{
    public required int Id { get; set; }
    /// <summary>
    /// ที่อยู่ : บ้านเลขที่ หมู่ ถนน ซอย อาคาร ให้รวมอยู่ในฟิลด์นี้
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// แขวง/ตำบล
    /// </summary>
    public string? SubDistrictId { get; set; }

    /// <summary>
    /// เขต/อำเภอ
    /// </summary>
    public string? DistrictId { get; set; }

    /// <summary>
    /// จังหวัด
    /// </summary>
    public string? ProvinceId { get; set; }

    /// <summary>
    /// รหัสไปรษณีย์
    /// </summary>
    public string? ZipCode { get; set; }
}

public class UpdateAdressPatientCommandHandler : IRequestHandler<UpdateAdressPatientCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateAdressPatientCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateAdressPatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Patient), request.Id);
        
        patient.Address = request.Address;
        patient.SubDistrictId = request.SubDistrictId;
        patient.DistrictId = request.DistrictId;
        patient.ProvinceId = request.ProvinceId;
        patient.ZipCode = request.ZipCode;

        _context.Patients.Update(patient);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
