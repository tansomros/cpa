using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.Hospital;

namespace BigLion.CPA.Application.Features.Hospitals.Commands.Update;

public class UpdateHospitalCommand : IRequest<Unit>
{
    public int HospitalUID { get; set; }
    public string? HospitalName { get; set; }
    public int? HospitalGroupID { get; set; }
    public int? HospitalTypeID { get; set; }
    public int? DepartmentID { get; set; }
    public string? DepartmentName { get; set; }
    public int? LevelID { get; set; }
    public int? Bed { get; set; }
    public int? Branch { get; set; }
    public string? Office_hours { get; set; }
    public int? Officer_Count { get; set; }
    public string? Address { get; set; }
    public string? ProvinceID { get; set; }
    public string? ProvinceName { get; set; }
    public string? ZipCode { get; set; }
    public string? Office_Tel { get; set; }
    public string? Office_Fax { get; set; }
    public string? Co_Name { get; set; }
    public string? Co_Position { get; set; }
    public string? Co_Mail { get; set; }
    public string? Co_Tel { get; set; }
    public string? StatusFlag { get; set; }
    public string? Bill_Name { get; set; }
    public int? WorkDayID { get; set; }
    public string? WorkDayDesc { get; set; }
    public int? WorkTimeID { get; set; }
    public string? WorkTimeDesc { get; set; }
    public string? ConfirmHold { get; set; }
    public int? isBranch { get; set; }
    public string? BranchRemark { get; set; }
    public string? WorkList { get; set; }
    public string? WorkSpec { get; set; }
    public string? WorkTop { get; set; }
    public string? Remark { get; set; }
    public string? Informant { get; set; }
    public string? InfoPosition { get; set; }
    public string? InfoDate { get; set; }
    public string? LetterTo { get; set; }
    public string? Country { get; set; }
    public string? ZoneID { get; set; }
    public string? OfficeID { get; set; }
    public string? Website { get; set; }
    public string? Facebook { get; set; }
    public string? Lat { get; set; }
    public string? Lng { get; set; }
    public DateTime? MWhen { get; set; }
    public string? MUser { get; set; }
}

public class UpdateHospitalCommandValidator : AbstractValidator<UpdateHospitalCommand>
{
    public UpdateHospitalCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateHospitalCommandHandler : IRequestHandler<UpdateHospitalCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateHospitalCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateHospitalCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Hospitals
            .FirstOrDefaultAsync(x => x.HospitalUID == request.HospitalUID, cancellationToken)
            ?? throw new NotFoundException("Hospital", request.HospitalUID);

        entity.HospitalName = request.HospitalName;
        entity.HospitalGroupID = request.HospitalGroupID;
        entity.HospitalTypeID = request.HospitalTypeID;
        entity.DepartmentID = request.DepartmentID;
        entity.DepartmentName = request.DepartmentName;
        entity.LevelID = request.LevelID;
        entity.Bed = request.Bed;
        entity.Branch = request.Branch;
        entity.Office_hours = request.Office_hours;
        entity.Officer_Count = request.Officer_Count;
        entity.Address = request.Address;
        entity.ProvinceID = request.ProvinceID;
        entity.ProvinceName = request.ProvinceName;
        entity.ZipCode = request.ZipCode;
        entity.Office_Tel = request.Office_Tel;
        entity.Office_Fax = request.Office_Fax;
        entity.Co_Name = request.Co_Name;
        entity.Co_Position = request.Co_Position;
        entity.Co_Mail = request.Co_Mail;
        entity.Co_Tel = request.Co_Tel;
        entity.StatusFlag = request.StatusFlag;
        entity.Bill_Name = request.Bill_Name;
        entity.WorkDayID = request.WorkDayID;
        entity.WorkDayDesc = request.WorkDayDesc;
        entity.WorkTimeID = request.WorkTimeID;
        entity.WorkTimeDesc = request.WorkTimeDesc;
        entity.ConfirmHold = request.ConfirmHold;
        entity.isBranch = request.isBranch;
        entity.BranchRemark = request.BranchRemark;
        entity.WorkList = request.WorkList;
        entity.WorkSpec = request.WorkSpec;
        entity.WorkTop = request.WorkTop;
        entity.Remark = request.Remark;
        entity.Informant = request.Informant;
        entity.InfoPosition = request.InfoPosition;
        entity.InfoDate = request.InfoDate;
        entity.LetterTo = request.LetterTo;
        entity.Country = request.Country;
        entity.ZoneID = request.ZoneID;
        entity.OfficeID = request.OfficeID;
        entity.Website = request.Website;
        entity.Facebook = request.Facebook;
        entity.Lat = request.Lat;
        entity.Lng = request.Lng;
        entity.MWhen = request.MWhen;
        entity.MUser = request.MUser;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
