using GBW.DataAccess;
using GBW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBW.Service
{
    public class GoldPackageEducationLinksService : Repository<GoldPackageEducationLinks>
    {
        private readonly ApplicationDbContext _context;
        public GoldPackageEducationLinksService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public bool DeleteEduLinksByPackage(List<GoldPackageEducationLinks> model)
        {
            if (model != null)
            {
                dbSet.RemoveRange(model);
                _context.SaveChanges();
            }
            return true;
        }
    }
}