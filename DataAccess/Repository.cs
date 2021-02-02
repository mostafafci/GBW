using GBW.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace GBW.DataAccess
{
    public class Repository<TEntity> where TEntity : class
    {
        public readonly ApplicationDbContext dbcontext;

        public DbSet<TEntity> dbSet;

        public Repository(ApplicationDbContext context)
        {
            dbcontext = context;
            dbSet = context.Set<TEntity>();
        }
        public virtual IQueryable<TEntity> Get()
        {
            return dbcontext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return dbcontext.Set<TEntity>();
        }

        public virtual void Delete(TEntity entity)
        {
            dbcontext.Set<TEntity>().Remove(entity);
        }

        public virtual void Insert(TEntity entity)
        {
            dbcontext.Set<TEntity>().Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            dbcontext.Set<TEntity>().AddOrUpdate(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entity)
        {
            dbcontext.Set<TEntity>().RemoveRange(entity);

        }

        public virtual void AddRange(IEnumerable<TEntity> entity)
        {
            dbcontext.Set<TEntity>().AddRange(entity);
        }

        public virtual int Count()
        {
            return dbSet.Count();
        }
    }
}