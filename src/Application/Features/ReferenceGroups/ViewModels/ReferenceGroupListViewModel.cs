using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.ReferenceGroups.ViewModels;

public class ReferenceGroupListViewModel
{
    public ICollection<ReferenceGroupViewModel> ReferenceGroups { get; set; }

    public ReferenceGroupListViewModel()
    {
        ReferenceGroups = new HashSet<ReferenceGroupViewModel>();
    }
}
