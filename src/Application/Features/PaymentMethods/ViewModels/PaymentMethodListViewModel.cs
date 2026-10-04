using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.PaymentMethods.ViewModels;

public class PaymentMethodListViewModel
{
    public ICollection<PaymentMethodViewModel> PaymentMethods { get; set; }

    public PaymentMethodListViewModel()
    {
        PaymentMethods = new HashSet<PaymentMethodViewModel>();
    }
}
