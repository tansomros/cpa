using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.Pharmacy.ViewModels;

public class PharmacyListViewModel
{
    public ICollection<PharmacyViewModel> Pharmacies { get; set; }

    public PharmacyListViewModel()
    {
        Pharmacies = new HashSet<PharmacyViewModel>();
    }
}
