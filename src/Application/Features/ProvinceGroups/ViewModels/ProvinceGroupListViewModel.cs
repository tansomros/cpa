using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.ProvinceGroups.ViewModels;

public class ProvinceGroupListViewModel
{
    public ICollection<ProvinceGroupViewModel> ProvinceGroups { get; set; }

    public ProvinceGroupListViewModel()
    {
        ProvinceGroups = new HashSet<ProvinceGroupViewModel>();
    }
}
