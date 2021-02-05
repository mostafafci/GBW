using GBW.Models;
using GBW.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBW.DataAccess
{
    public class UnitOfWork
    {
        private bool _disposed;

        public ApplicationDbContext Context { get; private set; }

        public UnitOfWork()
        {
            Context = new ApplicationDbContext();
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && Context != null)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        UsersService usersService;
        public UsersService UsersService => usersService ?? (usersService = new UsersService(Context));

        PartenersService partenersService;
        public PartenersService PartenersService => partenersService ?? (partenersService = new PartenersService(Context));
    }
}