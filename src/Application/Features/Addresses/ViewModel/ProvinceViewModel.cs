using Cpa.Application.Common.Interfaces;
using Cpa.Domain.Entities;

namespace Cpa.Application.Features.Addresses.ViewModel;
public class ProvinceViewModel : IMapFrom<Province>
{
    public string? ProvinceId { get; set; }
    public string? Name { get; set; }
    public string? NameEnglish { get; set; }
    public string? Region { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Province, ProvinceViewModel>();   
    }

}

public class ProvinceListViewModel
{
    public ICollection<ProvinceViewModel> Provinces { get; set; }
    public ProvinceListViewModel()
    {       
        Provinces = new HashSet<ProvinceViewModel>();
    }
}
