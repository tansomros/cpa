using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.DrugMasters.ViewModels;

public class DrugMasterListViewModel
{
    public ICollection<DrugMasterViewModel> DrugMasters { get; set; }

    public DrugMasterListViewModel()
    {
        DrugMasters = new HashSet<DrugMasterViewModel>();
    }
}
