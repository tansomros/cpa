using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.UserLogFile;

namespace BigLion.CPA.Application.Features.UserLogFiles.ViewModels;

public class UserLogFileViewModel : IMapFrom<Entity>
{
    public long LogID { get; set; }
    public int? UserID { get; set; }
    public DateTime? Work_Date { get; set; }
    public string? Act_Type { get; set; }
    public string? DB_Effective { get; set; }
    public string? Descrp { get; set; }
    public string? Remark { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, UserLogFileViewModel>();
    }
}
