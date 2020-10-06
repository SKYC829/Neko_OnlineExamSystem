using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Domain.Interfaces
{
    /// <summary>
    /// DAO的接口
    /// </summary>
    public interface IRepository
    {
    }

    /// <summary>
    /// DAO的接口
    /// </summary>
    /// <typeparam name="TDbEntity">数据库实体</typeparam>
    /// <typeparam name="TPrimaryKey">实体的主键类型</typeparam>
    public interface IRepository<TDbEntity, TPrimaryKey> : IRepository where TDbEntity :DbEntity<TPrimaryKey>
    {
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<TDbEntity> All();

        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="expression">获取数据的筛选条件</param>
        /// <returns></returns>
        IEnumerable<TDbEntity> All(Expression<Func<TDbEntity, bool>> expression);

        /// <summary>
        /// 加载一个数据
        /// </summary>
        /// <param name="id">数据的主键</param>
        /// <returns></returns>
        TDbEntity Load(TPrimaryKey id);

        /// <summary>
        /// 加载一个数据
        /// </summary>
        /// <param name="expression">加载数据的筛选条件</param>
        /// <returns></returns>
        TDbEntity Load(Expression<Func<TDbEntity, bool>> expression);

        /// <summary>
        /// 创建一个数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        TDbEntity Create(TDbEntity entity);

        /// <summary>
        /// 修改一个数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        TDbEntity Update(TDbEntity entity);

        /// <summary>
        /// 保存一个数据（如果元数据不存在就创建，否则修改。）
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        TDbEntity Save(TDbEntity entity);

        /// <summary>
        /// 提交数据库改动
        /// </summary>
        void Commit();

        /// <summary>
        /// 删除一个数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        void Remove(TDbEntity entity);

        /// <summary>
        /// 删除一个数据
        /// </summary>
        /// <param name="id">要删除的数据的主键</param>
        /// <returns></returns>
        void Remove(TPrimaryKey id);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="totalCount">总行数</param>
        /// <param name="whereBy">筛选条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        IQueryable<TDbEntity> Query(int pageIndex, int pageSize, out int totalCount, out int totalPage, Expression<Func<TDbEntity, bool>> whereBy, Expression<Func<TDbEntity, TPrimaryKey>> orderBy);
    }

    /// <summary>
    /// DAO的接口
    /// </summary>
    /// <typeparam name="TDbEntity">数据库实体</typeparam>
    public interface IRepository<TDbEntity> : IRepository<TDbEntity, int> where TDbEntity : DbEntity
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="totalCount">总行数</param>
        /// <param name="whereBy">筛选条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        IQueryable<TDbEntity> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<TDbEntity, bool>> whereBy, Expression<Func<TDbEntity, int>> orderBy);
    }
}
