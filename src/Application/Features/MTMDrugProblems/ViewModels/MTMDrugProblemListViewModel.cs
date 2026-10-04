using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.MTMDrugProblems.ViewModels;

public class MTMDrugProblemListViewModel
{
    public ICollection<MTMDrugProblemViewModel> MTMDrugProblems { get; set; }

    public MTMDrugProblemListViewModel()
    {
        MTMDrugProblems = new HashSet<MTMDrugProblemViewModel>();
    }
}
