using GBW.DataAccess;
using GBW.Models;
using GBW.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBW.Service
{
    public class EventsService : Repository<Events>
    {
        private readonly ApplicationDbContext _context;
        public EventsService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public List<EventsViewModel> GetAllEvents()
        {
            return dbSet.Where(s => s.IsDeleted == false).Select(s => new EventsViewModel()
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ImageBase64 = s.Image,
                StartDate=s.StartDate,
                EndDate=s.EndDate
            }).ToList();
        }


        public EventsViewModel GetEventDetails(int Id)
        {
            return dbSet.Where(s => s.IsDeleted == false && s.Id == Id).Select(s => new EventsViewModel()
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ImageBase64 = s.Image,
                StartDate=s.StartDate,
                EndDate=s.EndDate
            }).FirstOrDefault();

        }

        public bool AddEvent(EventsAddViewModel model, string UserId)
        {
            try
            {
                Events events = new Events();
                events.Name = model.Name;
                events.Description = model.Description;
                events.Image = model.ImageBase64;
                events.StartDate = model.StartDate;
                events.EndDate = model.EndDate;
                events.AddedBy = UserId;
                events.AddedDate = DateTime.Now;
                events.IsDeleted = false;
                dbSet.Add(events);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateEvent(EventsViewModel model)
        {
            var item = dbSet.Where(s => s.IsDeleted == false && s.Id == model.Id).FirstOrDefault();
            if (item == null)
            {
                return false;
            }
            item.Name = model.Name;
            item.Description = model.Description;
            item.Image = model.ImageBase64;
            item.StartDate = model.StartDate;
            item.EndDate = model.EndDate;
            _context.SaveChanges();
            return true;
        }

        public bool DeleteEvent(int Id)
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