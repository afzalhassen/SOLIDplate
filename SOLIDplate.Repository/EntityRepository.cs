using SOLIDplate.Common.Portable;
using SOLIDplate.Data.Interfaces;
using SOLIDplate.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SOLIDplate.Repository
{
    public abstract class EntityRepository<TEntity, TUnitOfWork> : IEntityRepository<TEntity>
        where TEntity : class, new()
        where TUnitOfWork : IUnitOfWork
    {
        private const string DefaultOrderingField = "Id";
        private const string DefaultOrderingDirection = "asc";

        protected readonly TUnitOfWork UnitOfWork;

        protected EntityRepository(TUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private static string FormatOrderClause(string orderingField, string orderingDirection)
        {
            return $"{orderingField} {orderingDirection}";
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            UnitOfWork.Add(entity);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            UnitOfWork.AddRange(entities);
        }

        public IQueryable<TEntity> All()
        {
            return UnitOfWork.All<TEntity>();
        }

        public IQueryable<TEntity> All(Expression<Func<TEntity, bool>> predicate)
        {
            return All().Where(predicate);
        }

        public IQueryable<TEntity> All(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageIndex)
        {
            return All(predicate, pageSize, pageIndex, DefaultOrderingField, DefaultOrderingDirection);
        }

        public IQueryable<TEntity> All(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageIndex, string orderingField, string orderingDirection)
        {
            var sortExpression = FormatOrderClause(orderingField, orderingDirection);
            return All(predicate).OrderBy(sortExpression)
                                 .Skip(pageSize * pageIndex)
                                 .Take(pageSize);
        }

        public TEntity One()
        {
            return All().SingleOrDefault();
        }

        public TEntity One(Expression<Func<TEntity, bool>> predicate)
        {
            return All().SingleOrDefault(predicate);
        }

        public int Count()
        {
            return AllUnwrapped().Count();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return All(predicate).Count();
        }

        public void Delete()
        {
            foreach (var entity in All())
            {
                Delete(entity);
            }
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            UnitOfWork.Delete(entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = All(predicate).ToArray();
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public abstract IQueryable<TEntity> AllUnwrapped();

        public IQueryable<TEntity> AllUnwrapped(Expression<Func<TEntity, bool>> predicate)
        {
            return AllUnwrapped().Where(predicate);
        }

        public IQueryable<TEntity> AllUnwrapped(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageIndex)
        {
            return AllUnwrapped(predicate, pageSize, pageIndex, DefaultOrderingField, DefaultOrderingDirection);
        }

        public IQueryable<TEntity> AllUnwrapped(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageIndex, string orderingField, string orderingDirection)
        {
            var sortExpression = FormatOrderClause(orderingField, orderingDirection);
            return AllUnwrapped(predicate).OrderBy(sortExpression)
                                          .Skip(pageSize * pageIndex)
                                          .Take(pageSize);
        }
    }
}
