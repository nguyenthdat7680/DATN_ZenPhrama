using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.Context;
using DA.ZenPharma.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DA.ZenPharma.Infrastructure.Repositories.Implementation
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ZenPharmaDbContext context) : base(context) { }
        public IQueryable<User> GetAllForPaging()
        {
            return _context.Users.AsNoTracking();
        }

        public async Task<List<User>> SearchByNameAsync(string keyword, int take)
        {
            return await _context.Users
                .Where(u => (u.FirstName + " " + u.LastName).Contains(keyword))
                .OrderByDescending(u => u.CreateDate)
                .Take(take)
                .ToListAsync();
        }
    }
}
