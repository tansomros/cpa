using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.MTMDrugRemains.ViewModels;

public class MTMDrugRemainListViewModel
{
    public ICollection<MTMDrugRemainViewModel> MTMDrugRemains { get; set; }

    public MTMDrugRemainListViewModel()
    {
        MTMDrugRemains = new HashSet<MTMDrugRemainViewModel>();
    }
}
