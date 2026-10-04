using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.Dispenses.ViewModels;

public class DispenseListViewModel
{
    public ICollection<DispenseViewModel> Dispenses { get; set; }

    public DispenseListViewModel()
    {
        Dispenses = new HashSet<DispenseViewModel>();
    }
}
