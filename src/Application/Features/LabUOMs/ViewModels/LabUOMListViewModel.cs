using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.LabUOMs.ViewModels;

public class LabUOMListViewModel
{
    public ICollection<LabUOMViewModel> LabUOMs { get; set; }

    public LabUOMListViewModel()
    {
        LabUOMs = new HashSet<LabUOMViewModel>();
    }
}
