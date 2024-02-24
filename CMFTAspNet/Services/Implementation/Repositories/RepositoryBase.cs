﻿using CMFTAspNet.Data;
using CMFTAspNet.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMFTAspNet.Services.Implementation.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class, IEntity
    {
        private readonly Func<CMFTAspNetContext, DbSet<T>> dbSetGetter;

        public RepositoryBase(CMFTAspNetContext context, Func<CMFTAspNetContext, DbSet<T>> dbSetGetter)
        {
            Context = context;
            this.dbSetGetter = dbSetGetter;
        }

        protected CMFTAspNetContext Context { get; private set; }


        public void Add(T item)
        {
            dbSetGetter(Context).Add(item);
        }

        public IEnumerable<T> GetAll()
        {
            return dbSetGetter(Context).ToList();
        }

        public virtual T? GetById(int id)
        {
            return dbSetGetter(Context).SingleOrDefault(t => t.Id == id);
        }

        public void Remove(int id)
        {
            var project = dbSetGetter(Context).Find(id);

            if (project != null)
            {
                dbSetGetter(Context).Remove(project);
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