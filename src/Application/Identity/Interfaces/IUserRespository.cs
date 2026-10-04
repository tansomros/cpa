using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigLion.CPA.Domain.Entities;

namespace BigLion.CPA.Application.Identity.Interfaces;
public interface IUserRepository
{
    Task<User?> FindByUsernameAsync(
        string username,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(
    CancellationToken cancellationToken);
    //Task UpdateAsync(
    //    User user,
    //    CancellationToken cancellationToken);
}
