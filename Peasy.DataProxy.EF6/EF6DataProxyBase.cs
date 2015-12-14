using AutoMapper;
using Peasy;
using Peasy.Core;
using Peasy.Exception;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.com.DAL.EF
{
    public abstract class EF6DataProxyBase<T, TKey> : IDataProxy<T, TKey> where T : class, IDomainObject<TKey>, new()
    {
        protected abstract DbContext GetDbContext();

        public virtual IEnumerable<T> GetAll()
        {
            using (var context = GetDbContext())
            {
                OnBeforeGetAllExecuted(context);
                var data = context.Set<T>().Select(Mapper.Map<T, T>).ToArray();
                OnAfterGetAllExecuted(context, data);
                return data;
            }
        }

        protected virtual void OnBeforeGetAllExecuted(DbContext context) { }
        protected virtual void OnAfterGetAllExecuted(DbContext context, IEnumerable<T> result) { }

        public virtual T GetByID(TKey id)
        {
            using (var context = GetDbContext())
            {
                OnBeforeGetByIDExecuted(context, id);
                var data = context.Set<T>().Find(id);
                OnAfterGetByIDExecuted(context, data);
                return Mapper.Map<T>(data);
            }
        }

        protected virtual void OnBeforeGetByIDExecuted(DbContext context, TKey id) { }
        protected virtual void OnAfterGetByIDExecuted(DbContext context, T result) { }

        public virtual T Insert(T entity)
        {
            using (var context = GetDbContext())
            {
                var data = Mapper.Map(entity, default(T));
                context.Set<T>().Add(data);
                OnBeforeInsertExecuted(context, data);
                context.SaveChanges();
                OnAfterInsertExecuted(context, entity);
                return Mapper.Map(data, entity);
            }
        }

        protected virtual void OnBeforeInsertExecuted(DbContext context, T entity) { }
        protected virtual void OnAfterInsertExecuted(DbContext context, T result) { }

        public virtual T Update(T entity)
        {
            using (var context = GetDbContext())
            {
                var data = Mapper.Map(entity, default(T));
                context.Entry<T>(data).State = EntityState.Modified;
                OnBeforeUpdateExecuted(context, data);
                context.SaveChanges();
                OnAfterUpdateExecuted(context, data);
                return Mapper.Map(data, entity);
            }
        }

        protected virtual void OnBeforeUpdateExecuted(DbContext context, T entity) { }
        protected virtual void OnAfterUpdateExecuted(DbContext context, T result) { }

        public virtual void Delete(TKey id)
        {
            using (var context = GetDbContext())
            {
                var entity = new T() { ID = id };
                context.Entry<T>(entity).State = EntityState.Deleted;
                OnBeforeDeleteExecuted(context, id);
                context.SaveChanges();
                OnAfterDeleteExecuted(context);
            }
        }

        protected virtual void OnBeforeDeleteExecuted(DbContext context, TKey id) { }
        protected virtual void OnAfterDeleteExecuted(DbContext context) { }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var context = GetDbContext())
            {
                await OnBeforeGetAllAsyncExecuted(context);
                var data = await context.Set<T>().ToListAsync();
                await OnAfterGetAllAsyncExecuted(context, data);
                return data.Select(Mapper.Map<T, T>).ToArray();
            }
        }

        protected virtual async Task OnBeforeGetAllAsyncExecuted(DbContext context) { }
        protected virtual async Task OnAfterGetAllAsyncExecuted(DbContext context, IEnumerable<T> result) { }

        public virtual async Task<T> GetByIDAsync(TKey id)
        {
            using (var context = GetDbContext())
            {
                await OnBeforeGetByIDExecutedAsync(context, id);
                var data = await context.Set<T>().FindAsync(id);
                await OnAfterGetByIDExecutedAsync(context, data);
                return Mapper.Map<T>(data);
            }
        }

        protected virtual async Task OnBeforeGetByIDExecutedAsync(DbContext context, TKey id) { }
        protected virtual async Task OnAfterGetByIDExecutedAsync(DbContext context, T result) { }

        public virtual async Task<T> InsertAsync(T entity)
        {
            using (var context = GetDbContext())
            {
                var data = Mapper.Map(entity, default(T));
                context.Set<T>().Add(data);
                await OnBeforeInsertExecutedAsync(context, data);
                await context.SaveChangesAsync();
                await OnAfterInsertExecutedAsync(context, data);
                return Mapper.Map(data, entity);
            }
        }

        protected virtual async Task OnBeforeInsertExecutedAsync(DbContext context, T entity) { }
        protected virtual async Task OnAfterInsertExecutedAsync(DbContext context, T result) { }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            using (var context = GetDbContext())
            {
                try
                {
                    var data = Mapper.Map(entity, default(T));
                    context.Entry<T>(data).State = EntityState.Modified;
                    await OnBeforeUpdateExecutedAsync(context, data);
                    await context.SaveChangesAsync();
                    await OnAfterUpdateExecutedAsync(context, data);
                    return Mapper.Map(data, entity);
                }
                catch (DbUpdateConcurrencyException ex) 
                {
                    string message = "A concurrency exception occurred";
                    throw new ConcurrencyException(message, ex);
                };
            }
        }

        protected virtual async Task OnBeforeUpdateExecutedAsync(DbContext context, T entity) { }
        protected virtual async Task OnAfterUpdateExecutedAsync(DbContext context, T result) { }

        public virtual async Task DeleteAsync(TKey id)
        {
            using (var context = GetDbContext())
            {
                var entity = new T() { ID = id };
                context.Entry<T>(entity).State = EntityState.Deleted;
                await OnBeforeDeleteExecutedAsync(context, id);
                await context.SaveChangesAsync();
                await OnAfterDeleteExecutedAsync(context);
            }
        }

        protected virtual async Task OnBeforeDeleteExecutedAsync(DbContext context, TKey id) { }
        protected virtual async Task OnAfterDeleteExecutedAsync(DbContext context) { }
    }
}
