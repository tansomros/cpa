using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.ServiceTypes.ViewModels;

public class ServiceTypeListViewModel
{
    public ICollection<ServiceTypeViewModel> ServiceTypes { get; set; }

    public ServiceTypeListViewModel()
    {
        ServiceTypes = new HashSet<ServiceTypeViewModel>();
    }
}
