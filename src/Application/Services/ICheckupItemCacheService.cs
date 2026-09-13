using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kondongpu.Application.Services;
public interface ICheckupItemCacheService
{
    Task<Dictionary<string, HashSet<string>>> GetAllOrderCodesAsync();
}
