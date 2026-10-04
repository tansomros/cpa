using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.MTMRefers.ViewModels;

public class MTMReferListViewModel
{
    public ICollection<MTMReferViewModel> MTMRefers { get; set; }

    public MTMReferListViewModel()
    {
        MTMRefers = new HashSet<MTMReferViewModel>();
    }
}
