using Cpa.Application.Common.Interfaces;
using Cpa.Application.Common.Security;
using Cpa.Domain.Entities;

#pragma warning disable CS0618
namespace Cpa.Application.Features.Systems.Commands;

/// <summary>
/// [OBSOLETE] ข้อมูลเหล่านี้ถูกแทนที่ด้วย SmartEnum ใน Domain.Enums แล้ว
/// ไม่ต้อง seed ลง DB อีกต่อไป — ใช้ ExamResult.All, LabResult.All ฯลฯ จาก memory ได้เลย
/// </summary>
[Obsolete("ข้อมูลเหล่านี้ถูกแทนที่ด้วย SmartEnum ใน Domain.Enums แล้ว ดู LookupRegistry.cs")]
[Authorize(Policy = CpaPolicies.AllowAnonymous)]
public class ReferenceValueDataInitializerCommand : IRequest<Unit> { }
public class ReferenceValueDataInitializerCommandHandler : IRequestHandler<ReferenceValueDataInitializerCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public ReferenceValueDataInitializerCommandHandler(ICpaDatabaseContext checkupDatabaseContext)
    {
        _context = checkupDatabaseContext;
    }

    public async Task<Unit> Handle(ReferenceValueDataInitializerCommand request, CancellationToken cancellationToken)
    {
        await SeedReferenceValues(cancellationToken); 
        return Unit.Value;
    }

    private async Task SeedReferenceValues(CancellationToken cancellationToken)
    {
        if (await _context.ReferenceValues.AnyAsync(cancellationToken))
        {
            return;
        }

        var ReferenceValue = new[]
        {
            new ReferenceValue("ไม่ได้ตรวจ","ไม่ได้ตรวจ",1,0),
            new ReferenceValue("Normal","ปกติ (Normal)",1,1),
            new ReferenceValue("Abnormal","ผิดปกติ (Abnormal)",1,2),
            new ReferenceValue("Normal","อยู่ในเกณฑ์ปกติ (Normal)",2,0),
            new ReferenceValue("Abnormal","ค่าผิดปกติ (Abnormal)",2,1),
            new ReferenceValue("Normal","ไม่พบความผิดปกติ",3,0),
            new ReferenceValue("Abnormal","ตรวจพบความผิดปกติ",3,1),
            new ReferenceValue("W","ส่งแพทย์เฉพาะทางอ่านผล",3,9),
            new ReferenceValue("Normal","มวลกระดูกอยู่ในเกณฑ์ปกติ",4,0),
            new ReferenceValue("C1","มวลกระดูกเริ่มบางเมื่อเทียบกับช่วงอายุเดียวกัน",4,1),
            new ReferenceValue("C2","กระดูกพรุนเมื่อเทียบกับช่วงอายุเดียวกัน",4,2),
            new ReferenceValue("Normal","อยู่ในเกณฑ์ปกติ",5,0),
            new ReferenceValue("C1","พบภาวะหลอดเลือดแข็ง",5,1),
            new ReferenceValue("C2","พบภาวะหลอดเลือดอุดตัน",5,2),
            new ReferenceValue("1","รอตรวจ",6,1),
            new ReferenceValue("2","กำลังตรวจ",6,2),
            new ReferenceValue("3","รายงานผล",6,3),
            new ReferenceValue("ปกติ","ปกติ",8,0),
            new ReferenceValue("สายตายาว","สายตายาว",8,1),
            new ReferenceValue("สายตาสั้น","สายตาสั้น",8,2),
            new ReferenceValue("ปกติ","ปกติ",9,0),
            new ReferenceValue("เสียเล็กน้อย","เสียเล็กน้อย",9,1),
            new ReferenceValue("เสียปานกลาง","เสียปานกลาง",9,2),
            new ReferenceValue("เสียมาก","เสียมาก",9,3),
            new ReferenceValue("เสียรุนแรง","เสียรุนแรง",9,4)
        };

        await _context.ReferenceValues.AddRangeAsync(ReferenceValue, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
     
}
