using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using Moq;

using FamousQuoteQuiz.Models;
using FamousQuoteQuiz.Data.Repositories;
using FamousQuoteQuiz.Data;

namespace FamousQuoteQuiz.Tests.Common
{
    public static class TestObjectFactory
    {
        public static Quote CreateQuote()
        {
            var author = CreateAuthor();
            var testObject = new Quote
            {
                Content = "I am cool",
                Author = author
            };

            return testObject;
        }

        public static Author CreateAuthor()
        {
            var testObject = new Author
            {
                Name = "Pesho"
            };

            return testObject;
        }

        public static QuizMode CreateQuizMode()
        {
            var testObject = new QuizMode
            {
                Type = QuizModeType.Binary,
                IsSelected = true
            };

            return testObject;
        }

        public static IRepository<T> CreateRepository<T>(List<T> testData) where T : BaseModel<int>
        {
            Mock<DbSet<T>> dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<T>(
                    testData.AsQueryable().GetEnumerator()));

            dbSetMock.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(testData.AsQueryable().Provider));

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).
                Returns(testData.AsQueryable().Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).
                Returns(testData.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).
                Returns(testData.AsQueryable().GetEnumerator());

            Mock<IQuoteQuizContext> contextMock = new Mock<IQuoteQuizContext>();
            contextMock.Setup(x => x.Set<T>()).Returns(dbSetMock.Object);

            IRepository<T> repositoryMock = new Repository<T>(contextMock.Object);

            return repositoryMock;
        }
    }
}
