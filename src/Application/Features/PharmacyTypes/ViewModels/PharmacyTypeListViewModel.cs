using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.PharmacyTypes.ViewModels;

public class PharmacyTypeListViewModel
{
    public ICollection<PharmacyTypeViewModel> PharmacyTypes { get; set; }

    public PharmacyTypeListViewModel()
    {
        PharmacyTypes = new HashSet<PharmacyTypeViewModel>();
    }
}
