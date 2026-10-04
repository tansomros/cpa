using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.Register;

namespace BigLion.CPA.Application.Features.Registers.Commands.Update;

public class UpdateRegisterCommand : IRequest<Unit>
{
    public int UID { get; set; }
    public string? LocationID { get; set; }
    public string? LicenseNo { get; set; }
    public string? LocationName { get; set; }
    public string? LocationName2 { get; set; }
    public string? NHSOCode { get; set; }
    public string? LocationType { get; set; }
    public string? LocationGroupID { get; set; }
    public string? Address { get; set; }
    public string? ProvinceID { get; set; }
    public string? ProvinceName { get; set; }
    public string? ZipCode { get; set; }
    public string? Office_Tel { get; set; }
    public string? Office_Mail { get; set; }
    public string? Office_Hour { get; set; }
    public string? LineID { get; set; }
    public string? Co_Name { get; set; }
    public string? Co_LicenseNo { get; set; }
    public string? Co_Mail { get; set; }
    public string? Co_Tel { get; set; }
    public string? RegisYear { get; set; }
    public string? Lat { get; set; }
    public string? Lng { get; set; }
    public int? RegisterStatus { get; set; }
    public DateTime? RegisterDate { get; set; }
    public int? MUser { get; set; }
    public DateTime? MWhen { get; set; }
}

public class UpdateRegisterCommandValidator : AbstractValidator<UpdateRegisterCommand>
{
    public UpdateRegisterCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateRegisterCommandHandler : IRequestHandler<UpdateRegisterCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateRegisterCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateRegisterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Registers
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("Register", request.UID);

        entity.LocationID = request.LocationID;
        entity.LicenseNo = request.LicenseNo;
        entity.LocationName = request.LocationName;
        entity.LocationName2 = request.LocationName2;
        entity.NHSOCode = request.NHSOCode;
        entity.LocationType = request.LocationType;
        entity.LocationGroupID = request.LocationGroupID;
        entity.Address = request.Address;
        entity.ProvinceID = request.ProvinceID;
        entity.ProvinceName = request.ProvinceName;
        entity.ZipCode = request.ZipCode;
        entity.Office_Tel = request.Office_Tel;
        entity.Office_Mail = request.Office_Mail;
        entity.Office_Hour = request.Office_Hour;
        entity.LineID = request.LineID;
        entity.Co_Name = request.Co_Name;
        entity.Co_LicenseNo = request.Co_LicenseNo;
        entity.Co_Mail = request.Co_Mail;
        entity.Co_Tel = request.Co_Tel;
        entity.RegisYear = request.RegisYear;
        entity.Lat = request.Lat;
        entity.Lng = request.Lng;
        entity.RegisterStatus = request.RegisterStatus;
        entity.RegisterDate = request.RegisterDate;
        entity.MUser = request.MUser;
        entity.MWhen = request.MWhen;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
