using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.Banks.ViewModels;

public class BankListViewModel
{
    public ICollection<BankViewModel> Banks { get; set; }

    public BankListViewModel()
    {
        Banks = new HashSet<BankViewModel>();
    }
}
