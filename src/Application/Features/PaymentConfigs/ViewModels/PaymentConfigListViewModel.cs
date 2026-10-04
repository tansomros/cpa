using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.PaymentConfigs.ViewModels;

public class PaymentConfigListViewModel
{
    public ICollection<PaymentConfigViewModel> PaymentConfigs { get; set; }

    public PaymentConfigListViewModel()
    {
        PaymentConfigs = new HashSet<PaymentConfigViewModel>();
    }
}
