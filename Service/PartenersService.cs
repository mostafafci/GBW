using GBW.DataAccess;
using GBW.Models;
using GBW.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBW.Service
{
    public class PartenersService : Repository<Parteners>
    {
        private readonly ApplicationDbContext _context;
        public PartenersService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public List<PartenersViewModel> GetAllPartners()
        {
            return dbSet.Where(s => s.IsDeleted == false).Select(s => new PartenersViewModel()
            {
                Id = s.Id,
                Name=s.Name,
                Description=s.Description,
                WebsiteURL=s.WebsiteURL,
                Image=s.Image
            }).ToList();
        }


        public PartenersViewModel GetPartnerDetails(int Id)
        {
            return dbSet.Where(s => s.IsDeleted == false && s.Id == Id).Select(s => new PartenersViewModel()
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                WebsiteURL = s.WebsiteURL,
                Image = s.Image
            }).FirstOrDefault();

        }

        public bool AddPartner(PartenersAddViewModel model,string UserId)
        {
            try
            {
                Parteners parteners = new Parteners();
                parteners.Name = model.Name;
                parteners.Description = model.Description;
                parteners.WebsiteURL = model.WebsiteURL;
                parteners.Image = model.Image;
                parteners.AddedBy = UserId;
                parteners.AddedDate = DateTime.Now;
                parteners.IsDeleted = false;
                dbSet.Add(parteners);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdatePartner(PartenersViewModel model)
        {
            var item = dbSet.Where(s => s.IsDeleted == false && s.Id == model.Id).FirstOrDefault();
            if (item == null)
            {
                return false;
            }
            item.Name = model.Name;
            item.Description = model.Description;
            item.WebsiteURL = model.WebsiteURL;
            item.Image = model.Image;
            _context.SaveChanges();
            return true;
        }

        public bool DeletePartner(int Id)
        {
            var item= dbSet.Where(s => s.IsDeleted == false && s.Id == Id).FirstOrDefault();
            if (item==null)
            {
                return false;
            }
            item.IsDeleted = true;
            _context.SaveChanges();
            return true;
        }

    }
}