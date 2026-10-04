using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.Provinces.ViewModels;

public class ProvinceListViewModel
{
    public ICollection<ProvinceViewModel> Provinces { get; set; }

    public ProvinceListViewModel()
    {
        Provinces = new HashSet<ProvinceViewModel>();
    }
}
