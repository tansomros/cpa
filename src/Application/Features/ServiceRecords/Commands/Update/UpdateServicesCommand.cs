using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.Services;

namespace BigLion.CPA.Application.Features.ServiceRecords.Commands.Update;

public class UpdateServicesCommand : IRequest<Unit>
{
    public long itemID { get; set; }
    public int? RefID { get; set; }
    public int? SeqNo { get; set; }
    public string? LocationID { get; set; }
    public int BYear { get; set; }
    public long PatientID { get; set; }
    public string ServiceTypeID { get; set; } = string.Empty;
    public int? EducateCount { get; set; }
    public long? ServiceDate { get; set; }
    public int? ServiceTime { get; set; }
    public int? PersonID { get; set; }
    public string? CustName { get; set; }
    public string? Gender { get; set; }
    public string? BirthDate { get; set; }
    public int? Ages { get; set; }
    public string? CardID { get; set; }
    public string? Telephone { get; set; }
    public string? Mobile { get; set; }
    public string? AddressType { get; set; }
    public string? AddressNo { get; set; }
    public string? Road { get; set; }
    public string? District { get; set; }
    public string? City { get; set; }
    public string? ProvinceID { get; set; }
    public string? ProvinceName { get; set; }
    public string? MainClaim { get; set; }
    public int? Status { get; set; }
    public DateTime? CloseDate { get; set; }
    public DateTime? LastUpdate { get; set; }
    public string? UpdBy { get; set; }
    public string? ChildName { get; set; }
    public string? ChildBirthDate { get; set; }
    public int? ChildAges { get; set; }
    public string? VaccineComplete { get; set; }
    public int? isEducate { get; set; }
    public int? isReview { get; set; }
    public string? EducateName { get; set; }
    public string? isPapSmear { get; set; }
    public int? isFollow { get; set; }
    public string? HospitalName { get; set; }
    public string? DateCheck { get; set; }
    public string? MedicinceDesc { get; set; }
    public string? DocFile { get; set; }
    public int? isNormal { get; set; }
    public string? Remark { get; set; }
    public int? isProblem1 { get; set; }
    public int? isProblem2 { get; set; }
    public int? isProblem3 { get; set; }
    public int? isProblem4 { get; set; }
    public int? isProblem5 { get; set; }
    public int? isProblem6 { get; set; }
    public int? isProblem7 { get; set; }
    public int? isProblem8 { get; set; }
    public int? isProblem9 { get; set; }
    public string? SubProblem9 { get; set; }
    public int? isProblem10 { get; set; }
    public string? SubProblem10 { get; set; }
    public int? isProblemOther { get; set; }
    public string? ProblemRemark { get; set; }
    public int? isEducate1 { get; set; }
    public int? isEducate2 { get; set; }
    public int? isEducate3 { get; set; }
    public int? isEducate4 { get; set; }
    public int? isEducate5 { get; set; }
    public int? isEducate6 { get; set; }
    public int? isEducate7 { get; set; }
    public int? isEducate8 { get; set; }
    public int? isEducate9 { get; set; }
    public int? isEducate10 { get; set; }
    public int? isEducateOther { get; set; }
    public string? EducateRemark { get; set; }
    public string? ProbMed1 { get; set; }
    public string? ProbPro1 { get; set; }
    public string? ProbMed2 { get; set; }
    public string? ProbPro2 { get; set; }
    public string? ProbMed3 { get; set; }
    public string? ProbPro3 { get; set; }
    public string? EduMed1 { get; set; }
    public string? EduPro1 { get; set; }
    public string? EduMed2 { get; set; }
    public string? EduPro2 { get; set; }
    public string? EduMed3 { get; set; }
    public string? EduPro3 { get; set; }
    public string? vct_Follow1 { get; set; }
    public string? vct_Follow2 { get; set; }
    public string? vct_FollowDate { get; set; }
    public string? ServicePlan { get; set; }
    public long? PayDate { get; set; }
    public long? CreateDate { get; set; }
    public string? InvoiceNo { get; set; }
    public string? Follow_Channel { get; set; }
    public int? Hospital_Type { get; set; }
    public string? NextDate { get; set; }
    public string? FollowDateSave { get; set; }
    public string? IsAbNormal { get; set; }
    public string? AbNormalRemark { get; set; }
    public string? ChildGender { get; set; }
    public int? isNHSO1 { get; set; }
    public int? isNHSO2 { get; set; }
    public int? isNHSO3 { get; set; }
    public int? isNHSO4 { get; set; }
    public int? isNHSO5 { get; set; }
    public int? isNHSO6 { get; set; }
    public int? isNHSO7 { get; set; }
    public int? isNHSO8 { get; set; }
    public int? isEducate11 { get; set; }
    public int? isEducate12 { get; set; }
    public string? CreateBy { get; set; }
    public string? ActiveStatus { get; set; }
    public double? TimeAfter { get; set; }
    public string? PatientFrom { get; set; }
    public string? isNotResponse { get; set; }
    public int? CauseResponse { get; set; }
    public string? OtherCause { get; set; }
    public string? PayRecordBy { get; set; }
    public DateTime? RWhen { get; set; }
}

public class UpdateServicesCommandValidator : AbstractValidator<UpdateServicesCommand>
{
    public UpdateServicesCommandValidator()
    {
        RuleFor(x => x.ServiceTypeID).NotEmpty();
    }
}

public class UpdateServicesCommandHandler : IRequestHandler<UpdateServicesCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateServicesCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateServicesCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Services
            .FirstOrDefaultAsync(x => x.itemID == request.itemID, cancellationToken)
            ?? throw new NotFoundException("Services", request.itemID);

        entity.RefID = request.RefID;
        entity.SeqNo = request.SeqNo;
        entity.LocationID = request.LocationID;
        entity.BYear = request.BYear;
        entity.PatientID = request.PatientID;
        entity.ServiceTypeID = request.ServiceTypeID;
        entity.EducateCount = request.EducateCount;
        entity.ServiceDate = request.ServiceDate;
        entity.ServiceTime = request.ServiceTime;
        entity.PersonID = request.PersonID;
        entity.CustName = request.CustName;
        entity.Gender = request.Gender;
        entity.BirthDate = request.BirthDate;
        entity.Ages = request.Ages;
        entity.CardID = request.CardID;
        entity.Telephone = request.Telephone;
        entity.Mobile = request.Mobile;
        entity.AddressType = request.AddressType;
        entity.AddressNo = request.AddressNo;
        entity.Road = request.Road;
        entity.District = request.District;
        entity.City = request.City;
        entity.ProvinceID = request.ProvinceID;
        entity.ProvinceName = request.ProvinceName;
        entity.MainClaim = request.MainClaim;
        entity.Status = request.Status;
        entity.CloseDate = request.CloseDate;
        entity.LastUpdate = request.LastUpdate;
        entity.UpdBy = request.UpdBy;
        entity.ChildName = request.ChildName;
        entity.ChildBirthDate = request.ChildBirthDate;
        entity.ChildAges = request.ChildAges;
        entity.VaccineComplete = request.VaccineComplete;
        entity.isEducate = request.isEducate;
        entity.isReview = request.isReview;
        entity.EducateName = request.EducateName;
        entity.isPapSmear = request.isPapSmear;
        entity.isFollow = request.isFollow;
        entity.HospitalName = request.HospitalName;
        entity.DateCheck = request.DateCheck;
        entity.MedicinceDesc = request.MedicinceDesc;
        entity.DocFile = request.DocFile;
        entity.isNormal = request.isNormal;
        entity.Remark = request.Remark;
        entity.isProblem1 = request.isProblem1;
        entity.isProblem2 = request.isProblem2;
        entity.isProblem3 = request.isProblem3;
        entity.isProblem4 = request.isProblem4;
        entity.isProblem5 = request.isProblem5;
        entity.isProblem6 = request.isProblem6;
        entity.isProblem7 = request.isProblem7;
        entity.isProblem8 = request.isProblem8;
        entity.isProblem9 = request.isProblem9;
        entity.SubProblem9 = request.SubProblem9;
        entity.isProblem10 = request.isProblem10;
        entity.SubProblem10 = request.SubProblem10;
        entity.isProblemOther = request.isProblemOther;
        entity.ProblemRemark = request.ProblemRemark;
        entity.isEducate1 = request.isEducate1;
        entity.isEducate2 = request.isEducate2;
        entity.isEducate3 = request.isEducate3;
        entity.isEducate4 = request.isEducate4;
        entity.isEducate5 = request.isEducate5;
        entity.isEducate6 = request.isEducate6;
        entity.isEducate7 = request.isEducate7;
        entity.isEducate8 = request.isEducate8;
        entity.isEducate9 = request.isEducate9;
        entity.isEducate10 = request.isEducate10;
        entity.isEducateOther = request.isEducateOther;
        entity.EducateRemark = request.EducateRemark;
        entity.ProbMed1 = request.ProbMed1;
        entity.ProbPro1 = request.ProbPro1;
        entity.ProbMed2 = request.ProbMed2;
        entity.ProbPro2 = request.ProbPro2;
        entity.ProbMed3 = request.ProbMed3;
        entity.ProbPro3 = request.ProbPro3;
        entity.EduMed1 = request.EduMed1;
        entity.EduPro1 = request.EduPro1;
        entity.EduMed2 = request.EduMed2;
        entity.EduPro2 = request.EduPro2;
        entity.EduMed3 = request.EduMed3;
        entity.EduPro3 = request.EduPro3;
        entity.vct_Follow1 = request.vct_Follow1;
        entity.vct_Follow2 = request.vct_Follow2;
        entity.vct_FollowDate = request.vct_FollowDate;
        entity.ServicePlan = request.ServicePlan;
        entity.PayDate = request.PayDate;
        entity.CreateDate = request.CreateDate;
        entity.InvoiceNo = request.InvoiceNo;
        entity.Follow_Channel = request.Follow_Channel;
        entity.Hospital_Type = request.Hospital_Type;
        entity.NextDate = request.NextDate;
        entity.FollowDateSave = request.FollowDateSave;
        entity.IsAbNormal = request.IsAbNormal;
        entity.AbNormalRemark = request.AbNormalRemark;
        entity.ChildGender = request.ChildGender;
        entity.isNHSO1 = request.isNHSO1;
        entity.isNHSO2 = request.isNHSO2;
        entity.isNHSO3 = request.isNHSO3;
        entity.isNHSO4 = request.isNHSO4;
        entity.isNHSO5 = request.isNHSO5;
        entity.isNHSO6 = request.isNHSO6;
        entity.isNHSO7 = request.isNHSO7;
        entity.isNHSO8 = request.isNHSO8;
        entity.isEducate11 = request.isEducate11;
        entity.isEducate12 = request.isEducate12;
        entity.CreateBy = request.CreateBy;
        entity.ActiveStatus = request.ActiveStatus;
        entity.TimeAfter = request.TimeAfter;
        entity.PatientFrom = request.PatientFrom;
        entity.isNotResponse = request.isNotResponse;
        entity.CauseResponse = request.CauseResponse;
        entity.OtherCause = request.OtherCause;
        entity.PayRecordBy = request.PayRecordBy;
        entity.RWhen = request.RWhen;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
