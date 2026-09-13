using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kondongpu.Application.Features.Checkups.ViewModels;

namespace Kondongpu.Application.Services;
public interface ICheckupStatusService
{
    Task UpdateStatusesAsync(List<CheckupViewModel> checkups);
}
