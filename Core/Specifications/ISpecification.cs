using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<TEntity>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; }
        public List<Expression<Func<TEntity, Object>>> Includes { get; }
        public Expression<Func<TEntity, Object>> OrderBy { get; }
        public Expression<Func<TEntity, Object>> OrderByDesc { get; }
        public int Skip { get; }
        public int Take { get; }
        public bool IsPagingEnabled { get; }


    }
}
