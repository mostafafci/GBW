using GBW.DataAccess;
using GBW.Models;
using GBW.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBW.Service
{
    public class GoldPackageService : Repository<GoldPackage>
    {
        private readonly ApplicationDbContext _context;
        public GoldPackageService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public List<GoldPackageViewModel> GetAllPackges()
        {
            return dbSet.Where(s => s.IsDeleted == false).Select(s => new GoldPackageViewModel()
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ImageBase64 = s.Image,
                NumberOfCards = s.NumberOfCards,
                NumberOfGram = s.NumberOfGram,
                Value = s.Value,
                RevenuePerMonth = s.RevenuePerMonth,
                EducationLinks = s.EducationLinks
            }).ToList();
        }


        public GoldPackageViewModel GetGoldPackageDetails(int Id)
        {
            return dbSet.Where(s => s.IsDeleted == false && s.Id == Id).Select(s => new GoldPackageViewModel()
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ImageBase64 = s.Image,
                NumberOfCards = s.NumberOfCards,
                NumberOfGram = s.NumberOfGram,
                Value = s.Value,
                RevenuePerMonth = s.RevenuePerMonth,
                EducationLinks = s.EducationLinks
            }).FirstOrDefault();

        }

        public bool AddGoldPackage(GoldPackageAddViewModel model, string UserId)
        {
            try
            {
                GoldPackage Gold = new GoldPackage();
                Gold.Name = model.Name;
                Gold.Description = model.Description;
                Gold.Image = model.ImageBase64;
                Gold.NumberOfCards = model.NumberOfCards;
                Gold.NumberOfGram = model.NumberOfGram;
                Gold.Value = model.Value;
                Gold.RevenuePerMonth = model.RevenuePerMonth;
                Gold.EducationLinks = model.EducationLinks.Select(x=>new GoldPackageEducationLinks() {Name=x.Name,EduLink=x.EduLink }).ToList();
                Gold.AddedBy = UserId;
                Gold.AddedDate = DateTime.Now;
                Gold.IsDeleted = false;
                dbSet.Add(Gold);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateGoldPackage(GoldPackageEditViewModel model)
        {
            var item = dbSet.Where(s => s.IsDeleted == false && s.Id == model.Id).FirstOrDefault();
            if (item == null)
            {
                return false;
            }
            item.Name = model.Name;
            item.Description = model.Description;
            item.Image = model.ImageBase64;
            item.NumberOfCards = model.NumberOfCards;
            item.NumberOfGram = model.NumberOfGram;
            item.Value = model.Value;
            item.RevenuePerMonth = model.RevenuePerMonth;
            item.EducationLinks = model.EducationLinks.Select(x=>new GoldPackageEducationLinks() {EduLink=x.EduLink,Name=x.Name }).ToList();
            _context.SaveChanges();
            return true;
        }

        public bool DeleteGoldPackage(int Id)
        {
            var item = dbSet.Where(s => s.IsDeleted == false && s.Id == Id).FirstOrDefault();
            if (item == null)
            {
                return false;
            }
            item.IsDeleted = true;
            _context.SaveChanges();
            return true;
        }
    }
}