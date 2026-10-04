using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Bank;

namespace BigLion.CPA.Application.Features.Banks.ViewModels;

public class BankViewModel : IMapFrom<Entity>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, BankViewModel>();
    }
}
