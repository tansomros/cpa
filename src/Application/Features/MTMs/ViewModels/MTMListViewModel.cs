using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.MTMs.ViewModels;

public class MTMListViewModel
{
    public ICollection<MTMViewModel> MTMs { get; set; }

    public MTMListViewModel()
    {
        MTMs = new HashSet<MTMViewModel>();
    }
}
