using Cpa.Application.Common.Interfaces;
using Cpa.Domain.Entities;

namespace Cpa.Application.Features.Addresses.ViewModel;
public class DistrictViewModel : IMapFrom<District>
{
    public string? DistrictId { get; set; }
    public string? Name { get; set; }
    public string? NameEnglish { get; set; }
    public string? ProvinceId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<District, DistrictViewModel>();   
    }  
}
  public class DistrictListViewModel
    {
        public ICollection<DistrictViewModel> Districts { get; set; }
        public DistrictListViewModel()
        { 
               Districts = new HashSet<DistrictViewModel>();
        }
    }
