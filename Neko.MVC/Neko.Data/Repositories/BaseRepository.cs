using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Neko.Domain;
using Neko.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Data.Repositories
{
    /// <summary>
    /// 数据库业务逻辑操作的基类(DAO基类)
    /// </summary>
    /// <typeparam name="TDbEntity">数据库实体类对象</typeparam>
    /// <typeparam name="TPrimaryKey">数据库实体的主键</typeparam>
    public abstract class BaseRepository<TDbEntity, TPrimaryKey> : IRepository<TDbEntity, TPrimaryKey> where TDbEntity : DbEntity<TPrimaryKey>
    {
        protected readonly NekoDbContext _dbContext;
        public BaseRepository(NekoDbContext context)
        {
            _dbContext = context;
        }
        public IEnumerable<TDbEntity> All()
        {
            return _dbContext.Set<TDbEntity>().ToList();
        }

        public IEnumerable<TDbEntity> All(Expression<Func<TDbEntity, bool>> expression)
        {
            return _dbContext.Set<TDbEntity>().Where(expression).ToList();
        }

        public async void Commit()
        {
           await _dbContext.SaveChangesAsync();
        }

        public TDbEntity Create(TDbEntity entity)
        {
            _dbContext.Set<TDbEntity>().Add(entity);
            Commit();
            return entity;
        }

        public TDbEntity Load(TPrimaryKey id)
        {
            return _dbContext.Set<TDbEntity>().FirstOrDefault(p => p.Id.Equals(id));
        }

        public TDbEntity Load(Expression<Func<TDbEntity, bool>> expression)
        {
            return _dbContext.Set<TDbEntity>().FirstOrDefault(expression);
        }

        public IQueryable<TDbEntity> Query(int pageIndex, int pageSize, out int totalCount, out int totalPage, Expression<Func<TDbEntity, bool>> whereBy, Expression<Func<TDbEntity, TPrimaryKey>> orderBy)
        {
            var result = _dbContext.Set<TDbEntity>().AsQueryable<TDbEntity>();
            if(whereBy != null)
            {
                result = result.Where(whereBy);
            }
            if(orderBy != null)
            {
                result = result.OrderBy(orderBy);
            }
            else
            {
                result = result.OrderBy(p => p.Id);
            }
            totalCount = result.Count();
            totalPage = Math.Max(1, totalCount / pageSize);
            return result.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public void Remove(TDbEntity entity)
        {
            _dbContext.Set<TDbEntity>().Remove(entity);
            Commit();
        }

        public void Remove(TPrimaryKey id)
        {
            TDbEntity entity = Load(id);
            Remove(entity);
        }

        public TDbEntity Save(TDbEntity entity)
        {
            if (entity.Id.Equals(0))
            {
                return Create(entity);
            }
            else
            {
                return Update(entity);
            }
        }

        public TDbEntity Update(TDbEntity entity)
        {
            _dbContext.Set<TDbEntity>().Attach(entity);
            entity = _dbContext.Entry(entity).Entity;
            _dbContext.Entry(entity).State = EntityState.Modified;
            Commit();
            return entity;
        }
    }

    /// <summary>
    /// 数据库业务逻辑操作的基类(DAO基类)
    /// </summary>
    /// <typeparam name="TDbEntity">数据库实体类对象</typeparam>
    public abstract class BaseRepository<TDbEntity> : BaseRepository<TDbEntity, int> where TDbEntity : DbEntity
    {
        public BaseRepository(NekoDbContext context) : base(context)
        {
        }
    }
}
