using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.MTMDeseases.ViewModels;

public class MTMDeseaseListViewModel
{
    public ICollection<MTMDeseaseViewModel> MTMDeseases { get; set; }

    public MTMDeseaseListViewModel()
    {
        MTMDeseases = new HashSet<MTMDeseaseViewModel>();
    }
}
