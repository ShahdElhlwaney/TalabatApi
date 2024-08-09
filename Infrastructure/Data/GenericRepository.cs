using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext context;

        public GenericRepository(StoreDbContext context)
        {
            this.context = context;
        }
        public void Add(T entity)
       =>context.Set<T>().Add(entity);

        public void Delete(T entity)
               => context.Set<T>().Remove(entity);

        public async Task<T> GetEntityWithSpecification(ISpecification<T> spec)
        => await ApplySpecification(spec).FirstOrDefaultAsync();
            public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        => await ApplySpecification(spec).ToListAsync();



        public IQueryable<T> ApplySpecification(ISpecification<T> spec)
        =>   SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);
        public async Task<IReadOnlyList<T>> GetAllAsync()
       => await context.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id)
        => await context.Set<T>().FindAsync(id);

        public void Update(T entity)
              => context.Set<T>().Update(entity);

        public async Task<int> CountAsync(ISpecification<T> spec)
              => await ApplySpecification(spec).CountAsync();

    }
}
