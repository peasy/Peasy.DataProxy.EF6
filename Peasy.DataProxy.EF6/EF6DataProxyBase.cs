using Peasy.Exception;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Peasy.DataProxy.EF6
{
    public abstract class EF6DataProxyBase<DTO, TEntity, TKey> : IDataProxy<DTO, TKey> where DTO : class, IDomainObject<TKey>, new()
                                                                                       where TEntity : class, IDomainObject<TKey>, new()
    {
        private static IMapper _mapper = new AutoMapperHelper();

        public EF6DataProxyBase()
        {
        }

        public EF6DataProxyBase(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected abstract DbContext GetDbContext();

        public virtual IEnumerable<DTO> GetAll()
        {
            using (var context = GetDbContext())
            {
                OnBeforeGetAllExecuted(context);
                var data = context.Set<TEntity>().Select(_mapper.Map<TEntity, DTO>).ToArray();
                OnAfterGetAllExecuted(context, data);
                return data;
            }
        }

        protected virtual void OnBeforeGetAllExecuted(DbContext context) { }
        protected virtual void OnAfterGetAllExecuted(DbContext context, IEnumerable<DTO> result) { }

        public virtual DTO GetByID(TKey id)
        {
            using (var context = GetDbContext())
            {
                OnBeforeGetByIDExecuted(context, id);
                var data = context.Set<TEntity>().Find(id);
                var result = _mapper.Map<TEntity, DTO>(data);
                OnAfterGetByIDExecuted(context, result);
                return result; 
            }
        }

        protected virtual void OnBeforeGetByIDExecuted(DbContext context, TKey id) { }
        protected virtual void OnAfterGetByIDExecuted(DbContext context, DTO result) { }

        public virtual DTO Insert(DTO entity)
        {
            using (var context = GetDbContext())
            {
                var data = _mapper.Map(entity, default(TEntity));
                context.Set<TEntity>().Add(data);
                OnBeforeInsertExecuted(context, data);
                context.SaveChanges();
                var result = _mapper.Map(data, entity);
                OnAfterInsertExecuted(context, entity);
                return result;
            }
        }

        protected virtual void OnBeforeInsertExecuted(DbContext context, TEntity entity) { }
        protected virtual void OnAfterInsertExecuted(DbContext context, DTO result) { }

        public virtual DTO Update(DTO entity)
        {
            using (var context = GetDbContext())
            {
                var data = _mapper.Map(entity, default(TEntity));
                context.Entry(data).State = EntityState.Modified;
                OnBeforeUpdateExecuted(context, data);
                context.SaveChanges();
                var result = _mapper.Map(data, entity);
                OnAfterUpdateExecuted(context, result);
                return result; 
            }
        }

        protected virtual void OnBeforeUpdateExecuted(DbContext context, TEntity entity) { }
        protected virtual void OnAfterUpdateExecuted(DbContext context, DTO result) { }

        public virtual void Delete(TKey id)
        {
            using (var context = GetDbContext())
            {
                var entity = new TEntity() { ID = id };
                context.Entry(entity).State = EntityState.Deleted;
                OnBeforeDeleteExecuted(context, id);
                context.SaveChanges();
                OnAfterDeleteExecuted(context);
            }
        }

        protected virtual void OnBeforeDeleteExecuted(DbContext context, TKey id) { }
        protected virtual void OnAfterDeleteExecuted(DbContext context) { }

        public virtual async Task<IEnumerable<DTO>> GetAllAsync()
        {
            using (var context = GetDbContext())
            {
                await OnBeforeGetAllAsyncExecuted(context);
                var data = await context.Set<TEntity>().ToListAsync();
                var result = data.Select(_mapper.Map<TEntity, DTO>).ToArray();
                await OnAfterGetAllAsyncExecuted(context, result);
                return result;
            }
        }

        protected virtual async Task OnBeforeGetAllAsyncExecuted(DbContext context) { }
        protected virtual async Task OnAfterGetAllAsyncExecuted(DbContext context, IEnumerable<DTO> result) { }

        public virtual async Task<DTO> GetByIDAsync(TKey id)
        {
            using (var context = GetDbContext())
            {
                await OnBeforeGetByIDExecutedAsync(context, id);
                var data = await context.Set<TEntity>().FindAsync(id);
                var result = _mapper.Map<TEntity, DTO>(data);
                await OnAfterGetByIDExecutedAsync(context, result);
                return result;
            }
        }

        protected virtual async Task OnBeforeGetByIDExecutedAsync(DbContext context, TKey id) { }
        protected virtual async Task OnAfterGetByIDExecutedAsync(DbContext context, DTO result) { }

        public virtual async Task<DTO> InsertAsync(DTO entity)
        {
            using (var context = GetDbContext())
            {
                var data = _mapper.Map(entity, default(TEntity));
                context.Set<TEntity>().Add(data);
                await OnBeforeInsertExecutedAsync(context, data);
                await context.SaveChangesAsync();
                var result = _mapper.Map(data, entity);
                await OnAfterInsertExecutedAsync(context, result);
                return result;
            }
        }

        protected virtual async Task OnBeforeInsertExecutedAsync(DbContext context, TEntity entity) { }
        protected virtual async Task OnAfterInsertExecutedAsync(DbContext context, DTO result) { }

        public virtual async Task<DTO> UpdateAsync(DTO entity)
        {
            using (var context = GetDbContext())
            {
                try
                {
                    var data = _mapper.Map(entity, default(TEntity));
                    context.Entry<TEntity>(data).State = EntityState.Modified;
                    await OnBeforeUpdateExecutedAsync(context, data);
                    await context.SaveChangesAsync();
                    var result = _mapper.Map(data, entity);
                    await OnAfterUpdateExecutedAsync(context, result);
                    return result;
                }
                catch (DbUpdateConcurrencyException ex) 
                {
                    string message = "A concurrency exception occurred";
                    throw new ConcurrencyException(message, ex);
                };
            }
        }

        protected virtual async Task OnBeforeUpdateExecutedAsync(DbContext context, TEntity entity) { }
        protected virtual async Task OnAfterUpdateExecutedAsync(DbContext context, DTO result) { }

        public virtual async Task DeleteAsync(TKey id)
        {
            using (var context = GetDbContext())
            {
                var entity = new TEntity() { ID = id };
                context.Entry<TEntity>(entity).State = EntityState.Deleted;
                await OnBeforeDeleteExecutedAsync(context, id);
                await context.SaveChangesAsync();
                await OnAfterDeleteExecutedAsync(context);
            }
        }

        protected virtual async Task OnBeforeDeleteExecutedAsync(DbContext context, TKey id) { }
        protected virtual async Task OnAfterDeleteExecutedAsync(DbContext context) { }
    }
}
