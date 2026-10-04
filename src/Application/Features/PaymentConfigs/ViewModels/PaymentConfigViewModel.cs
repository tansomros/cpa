using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.PaymentConfig;

namespace BigLion.CPA.Application.Features.PaymentConfigs.ViewModels;

public class PaymentConfigViewModel : IMapFrom<Entity>
{
    public int itemID { get; set; }
    public string? ProvinceID { get; set; }
    public int? PaymentID { get; set; }
    public long? EffectiveTo { get; set; }
    public string? StatusFlag { get; set; }
    public int? ProjectID { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, PaymentConfigViewModel>();
    }
}
