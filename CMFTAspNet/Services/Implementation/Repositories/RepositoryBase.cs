﻿using CMFTAspNet.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMFTAspNet.Services.Implementation.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class, IEntity
    {
        private readonly Func<AppContext, DbSet<T>> dbSetGetter;

        protected RepositoryBase(AppContext context, Func<AppContext, DbSet<T>> dbSetGetter)
        {
            Context = context;
            this.dbSetGetter = dbSetGetter;
        }

        protected AppContext Context { get; private set; }


        public void Add(T item)
        {
            dbSetGetter(Context).Add(item);
        }

        public bool Exists(int id)
        {
            return dbSetGetter(Context).Find(id) != null;
        }

        public bool Exists(Func<T, bool> predicate)
        {
            return GetByPredicate(predicate) != null;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSetGetter(Context).ToList();
        }

        public virtual T? GetById(int id)
        {
            return dbSetGetter(Context).SingleOrDefault(t => t.Id == id);
        }

        public virtual T? GetByPredicate(Func<T, bool> predicate)
        {
            return dbSetGetter(Context).SingleOrDefault(predicate);
        }

        public void Remove(int id)
        {
            var itemToRemove = dbSetGetter(Context).Find(id);

            if (itemToRemove != null)
            {
                dbSetGetter(Context).Remove(itemToRemove);
            }
        }

        public async Task Save()
        {
            await Context.SaveChangesAsync();
        }

        public void Update(T item)
        {
            Context.Entry(item).State = EntityState.Modified;
        }
    }
}