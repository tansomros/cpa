using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.UserLogFiles.ViewModels;

public class UserLogFileListViewModel
{
    public ICollection<UserLogFileViewModel> UserLogFiles { get; set; }

    public UserLogFileListViewModel()
    {
        UserLogFiles = new HashSet<UserLogFileViewModel>();
    }
}
