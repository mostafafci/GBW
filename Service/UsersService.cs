using GBW.DataAccess;
using GBW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBW.Service
{
    public class UsersService : Repository<ApplicationUser>
    {
        private readonly ApplicationDbContext _context;
        public UsersService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public ApplicationUser GetLoginByEmail(string email)
        {
            return dbSet.FirstOrDefault(s => s.Email == email);
        }

        public ApplicationUser GetByEmail(string email)
        {
            return dbSet.FirstOrDefault(s => s.Email == email);
        }
    }
}