using Core.Entities;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T: BaseEntity 
    {
        public  Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        public  Task<T> GetEntityWithSpecification(ISpecification<T> spec);

       public Task<T> GetByIdAsync(int id);
        public Task<int> CountAsync(ISpecification<T> spec);
       public Task<IReadOnlyList<T>> GetAllAsync();
       public void Add(T entity);
       public void Update(T entity);
       public void Delete(T entity);
    }
}
