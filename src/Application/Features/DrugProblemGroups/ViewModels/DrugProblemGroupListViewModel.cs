using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.DrugProblemGroups.ViewModels;

public class DrugProblemGroupListViewModel
{
    public ICollection<DrugProblemGroupViewModel> DrugProblemGroups { get; set; }

    public DrugProblemGroupListViewModel()
    {
        DrugProblemGroups = new HashSet<DrugProblemGroupViewModel>();
    }
}
