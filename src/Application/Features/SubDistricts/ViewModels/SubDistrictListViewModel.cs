using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.SubDistricts.ViewModels;

public class SubDistrictListViewModel
{
    public ICollection<SubDistrictViewModel> SubDistricts { get; set; }

    public SubDistrictListViewModel()
    {
        SubDistricts = new HashSet<SubDistrictViewModel>();
    }
}
