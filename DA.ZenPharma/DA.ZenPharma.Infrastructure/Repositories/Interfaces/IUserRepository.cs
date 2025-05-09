using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;

namespace DA.ZenPharma.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        IQueryable<User> GetAllForPaging();
        Task<List<User>> SearchByNameAsync(string keyword, int take);
    }
}
