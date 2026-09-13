using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cpa.Domain.Entities;

namespace Cpa.Application.Identity.Interfaces;
public interface IJwtTokenService
{
    string GenerateToken(User user);
}
