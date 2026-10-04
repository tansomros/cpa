using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.DrugProblemItems.ViewModels;

public class DrugProblemItemListViewModel
{
    public ICollection<DrugProblemItemViewModel> DrugProblemItems { get; set; }

    public DrugProblemItemListViewModel()
    {
        DrugProblemItems = new HashSet<DrugProblemItemViewModel>();
    }
}
