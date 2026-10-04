using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.MTMBehaviors.ViewModels;

public class MTMBehaviorListViewModel
{
    public ICollection<MTMBehaviorViewModel> MTMBehaviors { get; set; }

    public MTMBehaviorListViewModel()
    {
        MTMBehaviors = new HashSet<MTMBehaviorViewModel>();
    }
}
