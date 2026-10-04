using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.Deseases.ViewModels;

public class DeseaseListViewModel
{
    public ICollection<DeseaseViewModel> Deseases { get; set; }

    public DeseaseListViewModel()
    {
        Deseases = new HashSet<DeseaseViewModel>();
    }
}
