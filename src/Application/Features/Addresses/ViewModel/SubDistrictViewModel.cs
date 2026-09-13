using Cpa.Application.Common.Interfaces;
using Cpa.Domain.Entities;

namespace Cpa.Application.Features.Addresses.ViewModel;
public class SubDistrictViewModel : IMapFrom<SubDistrict>
{
    public string? SubDistrictId { get; set; }
    public string? Name { get; set; }
    public string? NameEnglish { get; set; }
    public string? DistrictId { get; set; }
    //public DistrictViewModel? Districts { get; set; }
    public string? ProvinceId { get; set; }
    //public ProvinceViewModel? Provinces { get; set; }
    public string? ZipCode { get; set; } 

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SubDistrict, SubDistrictViewModel>();
    }  
}

public class SubDistrictListViewModel
{
    public ICollection<SubDistrictViewModel> SubDistricts { get; set; }
    public SubDistrictListViewModel()
    {
        SubDistricts = new HashSet<SubDistrictViewModel>();
    }
}
