using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Moq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Moq.Protected;

namespace Peasy.DataProxy.EF6.Tests
{
    [TestClass]
    public class EF6DataProxyBaseTests
    {
        [TestMethod]
        public void ensure_method_invocations_for_GetAll()
        {
            var mockContext = new Mock<Context>();
            mockContext.Setup(m => m.Set<Person>()).Returns(GetMockSet());
            var proxy = new ProxyStub(mockContext.Object);
            proxy.GetAll();
            proxy.OnBeforeGetAllExecutedWasInvoked.ShouldBe(true);
            proxy.OnAfterGetAllExecutedWasInvoked.ShouldBe(true);
        }

        [TestMethod]
        public void ensure_method_invocations_for_GetByID()
        {
            var mockContext = new Mock<Context>();
            mockContext.Setup(m => m.Set<Person>()).Returns(GetMockSet());
            var proxy = new ProxyStub(mockContext.Object);

            proxy.GetByID(1);

            proxy.OnBeforeGetByIDExecutedWasInvoked.ShouldBe(true);
            proxy.OnAfterGetByIDExecutedWasInvoked.ShouldBe(true);
        }

        [TestMethod]
        public void ensure_method_invocations_for_Insert()
        {
            ProxyStub proxy = BuildProxy();

            proxy.Insert(new Person { ID = 1 });

            proxy.OnBeforeInsertExecutedWasInvoked.ShouldBe(true);
            proxy.OnAfterInsertExecutedWasInvoked.ShouldBe(true);
        }

        [TestMethod]
        public void ensure_method_invocations_for_Update()
        {
            var context = new Context();
            var proxy = new ProxyStub(context);


            //ProxyStub proxy = BuildProxy();

            proxy.Update(new Person { ID = 1 });

            proxy.OnBeforeUpdateExecutedWasInvoked.ShouldBe(true);
            proxy.OnAfterUpdateExecutedWasInvoked.ShouldBe(true);
        }

        [TestMethod]
        public void ensure_method_invocations_for_Delete()
        {
            ProxyStub proxy = BuildProxy();

            proxy.Delete(1);

            proxy.OnBeforeDeleteExecutedWasInvoked.ShouldBe(true);
            proxy.OnAfterDeleteExecutedWasInvoked.ShouldBe(true);
        }

        private ProxyStub BuildProxy()
        {
            var mockContext = new Mock<Context>();
            mockContext.Setup(m => m.Set<Person>()).Returns(GetMockSet());
            mockContext.Setup(m => m.Entry<Person>(It.IsAny<Person>()));
            //mockContext.Protected()
            //           .Setup<Person>("Entry", ItExpr.IsAny<Person>());
            var proxy = new ProxyStub(mockContext.Object);
            return proxy;
        }

        private DbSet<Person> GetMockSet()
        {
            var data = new List<Person>
            {
                new Person { ID = 1 },
                new Person { ID = 2 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Person>>();
            mockSet.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet.Object;
        }
    }
}
