using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.LabItems.ViewModels;

public class LabItemListViewModel
{
    public ICollection<LabItemViewModel> LabItems { get; set; }

    public LabItemListViewModel()
    {
        LabItems = new HashSet<LabItemViewModel>();
    }
}
