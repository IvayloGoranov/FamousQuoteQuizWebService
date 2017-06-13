using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

using Moq;

using FamousQuoteQuiz.Data.Repositories;
using FamousQuoteQuiz.Models;
using FamousQuoteQuiz.Tests.Common;

namespace FamousQuoteQuiz.Data.Tests
{
    [TestClass]
    public class RepositoryTest
    {
        private Mock<IQuoteQuizContext> contextMock;
        private IRepository<Quote> quoteRepository;
        private Mock<Quote> quoteMock;
        private Mock<DbSet<Quote>> dbSetMock;
        private List<Quote> testData;

        [TestInitialize]
        public void Init()
        {
            this.quoteMock = new Mock<Quote>();
            this.quoteMock.Setup(x => x.Id).Returns(1);
            this.testData = new List<Quote>();
            testData.Add(quoteMock.Object);

            this.dbSetMock = new Mock<DbSet<Quote>>();
            this.dbSetMock.As<IDbAsyncEnumerable<Quote>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Quote>(
                    this.testData.AsQueryable().GetEnumerator()));
            this.dbSetMock.As<IQueryable<Quote>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Quote>(this.testData.AsQueryable().Provider));

            this.dbSetMock.As<IQueryable<Quote>>().Setup(m => m.Expression).
                Returns(this.testData.AsQueryable().Expression);
            this.dbSetMock.As<IQueryable<Quote>>().Setup(m => m.ElementType).
                Returns(this.testData.AsQueryable().ElementType);
            this.dbSetMock.As<IQueryable<Quote>>().Setup(m => m.GetEnumerator()).
                Returns(this.testData.AsQueryable().GetEnumerator());

            this.contextMock = new Mock<IQuoteQuizContext>();
            this.contextMock.Setup(x => x.Set<Quote>()).Returns(dbSetMock.Object);

            this.quoteRepository = new Repository<Quote>(contextMock.Object);
        }

        [TestMethod]
        public async Task TestAdd_ShouldAddEntityToDbSetAndCallSaveChangesAsync()
        {
            await this.quoteRepository.Add(this.quoteMock.Object);

            this.dbSetMock.
                Verify(x => x.Add(It.Is<Quote>(b => b.Equals(this.quoteMock.Object))), Times.Once());
            this.contextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestMethod]
        public async Task TestDelete_ShouldRemoveEntityFromDbSetAndCallSaveChangesAsync()
        {
            await this.quoteRepository.Delete(this.quoteMock.Object);

            this.dbSetMock.
                Verify(x => x.Remove(It.Is<Quote>(b => b.Equals(this.quoteMock.Object))), Times.Once());
            this.contextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestMethod]
        public async Task TestFind_ShouldReturnCorrectEntity()
        {
            var bookingFound = await this.quoteRepository.Find(1);

            Assert.AreEqual(bookingFound, quoteMock.Object);
        }

        [TestMethod]
        public void TestGetAll_ShouldReturnCorrectEntities()
        {
            var bookingsFound = this.quoteRepository.GetAll().ToList();

            Assert.AreEqual(bookingsFound[0], quoteMock.Object);
        }
    }
}
