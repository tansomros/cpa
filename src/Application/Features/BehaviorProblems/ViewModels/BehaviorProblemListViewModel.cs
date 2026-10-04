using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.BehaviorProblems.ViewModels;

public class BehaviorProblemListViewModel
{
    public ICollection<BehaviorProblemViewModel> BehaviorProblems { get; set; }

    public BehaviorProblemListViewModel()
    {
        BehaviorProblems = new HashSet<BehaviorProblemViewModel>();
    }
}
