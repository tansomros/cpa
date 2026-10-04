using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.Districts.ViewModels;

public class DistrictListViewModel
{
    public ICollection<DistrictViewModel> Districts { get; set; }

    public DistrictListViewModel()
    {
        Districts = new HashSet<DistrictViewModel>();
    }
}
