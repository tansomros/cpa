using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.ServiceRecords.ViewModels;

public class ServicesListViewModel
{
    public ICollection<ServicesViewModel> Services { get; set; }

    public ServicesListViewModel()
    {
        Services = new HashSet<ServicesViewModel>();
    }
}
