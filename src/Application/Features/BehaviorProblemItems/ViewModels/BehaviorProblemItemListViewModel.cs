using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.BehaviorProblemItems.ViewModels;

public class BehaviorProblemItemListViewModel
{
    public ICollection<BehaviorProblemItemViewModel> BehaviorProblemItems { get; set; }

    public BehaviorProblemItemListViewModel()
    {
        BehaviorProblemItems = new HashSet<BehaviorProblemItemViewModel>();
    }
}
