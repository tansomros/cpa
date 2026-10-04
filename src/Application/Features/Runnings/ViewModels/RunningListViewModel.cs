using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.Runnings.ViewModels;

public class RunningListViewModel
{
    public ICollection<RunningViewModel> Runnings { get; set; }

    public RunningListViewModel()
    {
        Runnings = new HashSet<RunningViewModel>();
    }
}
