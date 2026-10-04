using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.PaymentMethod;

namespace BigLion.CPA.Application.Features.PaymentMethods.ViewModels;

public class PaymentMethodViewModel : IMapFrom<Entity>
{
    public int PaymentID { get; set; }
    public string? PaymentName { get; set; }
    public double? Amount { get; set; }
    public long? EffectiveTo { get; set; }
    public string? StatusFlag { get; set; }
    public string? ServiceTypeID { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, PaymentMethodViewModel>();
    }
}
