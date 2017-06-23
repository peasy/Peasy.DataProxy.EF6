using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Peasy.DataProxy.EF6.Tests
{
    public class Person : IDomainObject<long>
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }

    public class Context : DbContext
    {
        public virtual FakeDbSet<Person> People { get; set; }

        public new DbEntityEntry Entry(object entity)
        {
            return null;
        }
    }

    public class ProxyStub : EF6DataProxyBase<Person, Person, long>
    {
        private DbContext _context;

        public bool OnAfterDeleteExecutedAsyncWasInvoked { get; private set; }
        public bool OnAfterDeleteExecutedWasInvoked { get; private set; }
        public bool OnAfterGetAllAsyncExecutedWasInvoked { get; private set; }
        public bool OnAfterGetAllExecutedWasInvoked { get; private set; }
        public bool OnAfterGetByIDExecutedAsyncWasInvoked { get; private set; }
        public bool OnAfterGetByIDExecutedWasInvoked { get; private set; }
        public bool OnAfterInsertExecutedAsyncWasInvoked { get; private set; }
        public bool OnAfterInsertExecutedWasInvoked { get; private set; }
        public bool OnAfterUpdateExecutedAsyncWasInvoked { get; private set; }
        public bool OnAfterUpdateExecutedWasInvoked { get; private set; }
        public bool OnBeforeDeleteExecutedAsyncWasInvoked { get; private set; }
        public bool OnBeforeDeleteExecutedWasInvoked { get; private set; }
        public bool OnBeforeGetAllAsyncExecutedWasInvoked { get; private set; }
        public bool OnBeforeGetAllExecutedWasInvoked { get; private set; }
        public bool OnBeforeGetByIDExecutedAsyncWasInvoked { get; private set; }
        public bool OnBeforeGetByIDExecutedWasInvoked { get; private set; }
        public bool OnBeforeInsertExecutedAsyncWasInvoked { get; private set; }
        public bool OnBeforeInsertExecutedWasInvoked { get; private set; }
        public bool OnBeforeUpdateExecutedAsyncWasInvoked { get; private set; }
        public bool OnBeforeUpdateExecutedWasInvoked { get; private set; }

        public ProxyStub()
        {
        }

        public ProxyStub(DbContext context)
        {
            _context = context;
        }

        protected override DbContext GetDbContext()
        {
            return _context;
        }

        protected override void OnAfterDeleteExecuted(DbContext context)
        {
            OnAfterDeleteExecutedWasInvoked = true;
            base.OnAfterDeleteExecuted(context);
        }

        protected override Task OnAfterDeleteExecutedAsync(DbContext context)
        {
            OnAfterDeleteExecutedAsyncWasInvoked = true;
            return base.OnAfterDeleteExecutedAsync(context);
        }

        protected override Task OnAfterGetAllAsyncExecuted(DbContext context, IEnumerable<Person> result)
        {
            OnAfterGetAllAsyncExecutedWasInvoked = true;
            return base.OnAfterGetAllAsyncExecuted(context, result);
        }

        public override IEnumerable<Person> GetAll()
        {
            return base.GetAll();

        }
        protected override void OnAfterGetAllExecuted(DbContext context, IEnumerable<Person> result)
        {
            OnAfterGetAllExecutedWasInvoked = true;
            base.OnAfterGetAllExecuted(context, result);
        }

        protected override void OnAfterGetByIDExecuted(DbContext context, Person result)
        {
            OnAfterGetByIDExecutedWasInvoked = true;
            base.OnAfterGetByIDExecuted(context, result);
        }

        protected override Task OnAfterGetByIDExecutedAsync(DbContext context, Person result)
        {
            OnAfterGetByIDExecutedAsyncWasInvoked = true;
            return base.OnAfterGetByIDExecutedAsync(context, result);
        }

        protected override void OnAfterInsertExecuted(DbContext context, Person result)
        {
            OnAfterInsertExecutedWasInvoked = true;
            base.OnAfterInsertExecuted(context, result);
        }

        protected override Task OnAfterInsertExecutedAsync(DbContext context, Person result)
        {
            OnAfterInsertExecutedAsyncWasInvoked = true;
            return base.OnAfterInsertExecutedAsync(context, result);
        }

        protected override void OnAfterUpdateExecuted(DbContext context, Person result)
        {
            OnAfterUpdateExecutedWasInvoked = true;
            base.OnAfterUpdateExecuted(context, result);
        }

        protected override Task OnAfterUpdateExecutedAsync(DbContext context, Person result)
        {
            OnAfterUpdateExecutedAsyncWasInvoked = true;
            return base.OnAfterUpdateExecutedAsync(context, result);
        }

        protected override void OnBeforeDeleteExecuted(DbContext context, long id)
        {
            OnBeforeDeleteExecutedWasInvoked = true;
            base.OnBeforeDeleteExecuted(context, id);
        }

        protected override Task OnBeforeDeleteExecutedAsync(DbContext context, long id)
        {
            OnBeforeDeleteExecutedAsyncWasInvoked = true;
            return base.OnBeforeDeleteExecutedAsync(context, id);
        }

        protected override Task OnBeforeGetAllAsyncExecuted(DbContext context)
        {
            OnBeforeGetAllAsyncExecutedWasInvoked = true;
            return base.OnBeforeGetAllAsyncExecuted(context);
        }

        protected override void OnBeforeGetAllExecuted(DbContext context)
        {
            OnBeforeGetAllExecutedWasInvoked = true;
            base.OnBeforeGetAllExecuted(context);
        }

        protected override void OnBeforeGetByIDExecuted(DbContext context, long id)
        {
            OnBeforeGetByIDExecutedWasInvoked = true;
            base.OnBeforeGetByIDExecuted(context, id);
        }

        protected override Task OnBeforeGetByIDExecutedAsync(DbContext context, long id)
        {
            OnBeforeGetByIDExecutedAsyncWasInvoked = true;
            return base.OnBeforeGetByIDExecutedAsync(context, id);
        }

        protected override void OnBeforeInsertExecuted(DbContext context, Person entity)
        {
            OnBeforeInsertExecutedWasInvoked = true;
            base.OnBeforeInsertExecuted(context, entity);
        }

        protected override Task OnBeforeInsertExecutedAsync(DbContext context, Person entity)
        {
            OnBeforeInsertExecutedAsyncWasInvoked = true;
            return base.OnBeforeInsertExecutedAsync(context, entity);
        }

        protected override void OnBeforeUpdateExecuted(DbContext context, Person entity)
        {
            OnBeforeUpdateExecutedWasInvoked = true;
            base.OnBeforeUpdateExecuted(context, entity);
        }

        protected override Task OnBeforeUpdateExecutedAsync(DbContext context, Person entity)
        {
            OnBeforeUpdateExecutedAsyncWasInvoked = true;
            return base.OnBeforeUpdateExecutedAsync(context, entity);
        }
    }
}
