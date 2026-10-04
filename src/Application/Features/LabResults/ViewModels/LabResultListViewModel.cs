using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.LabResults.ViewModels;

public class LabResultListViewModel
{
    public ICollection<LabResultViewModel> LabResults { get; set; }

    public LabResultListViewModel()
    {
        LabResults = new HashSet<LabResultViewModel>();
    }
}
