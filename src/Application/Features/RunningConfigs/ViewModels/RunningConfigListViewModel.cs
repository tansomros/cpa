using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.RunningConfigs.ViewModels;

public class RunningConfigListViewModel
{
    public ICollection<RunningConfigViewModel> RunningConfigs { get; set; }

    public RunningConfigListViewModel()
    {
        RunningConfigs = new HashSet<RunningConfigViewModel>();
    }
}
