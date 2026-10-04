using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.Hospitals.ViewModels;

public class HospitalListViewModel
{
    public ICollection<HospitalViewModel> Hospitals { get; set; }

    public HospitalListViewModel()
    {
        Hospitals = new HashSet<HospitalViewModel>();
    }
}
