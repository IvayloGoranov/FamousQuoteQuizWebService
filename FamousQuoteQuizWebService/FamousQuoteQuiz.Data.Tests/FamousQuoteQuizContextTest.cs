using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;

using FamousQuoteQuiz.Tests.Common;

namespace FamousQuoteQuiz.Data.Tests
{
    [TestClass]
    public class FamousQuoteQuizContextTest
    {
        private TransactionScope transactionScope;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            QuoteQuizContext dbContext = new QuoteQuizContext();
            dbContext.Database.Delete();
            dbContext.Database.Create();
        }

        [AssemblyCleanup]
        public static void AssemlbyCleanup()
        {
            QuoteQuizContext dbContext = new QuoteQuizContext();
            dbContext.Database.Delete();
        }

        [TestInitialize]
        public void TestInit()
        {
            this.transactionScope = new TransactionScope();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            this.transactionScope.Dispose();
        }

        [TestMethod]
        public void TestQuote_ShouldAddEntityToDbSetAndDbCorrectly()
        {
            QuoteQuizContext dbContext = new QuoteQuizContext();

            var quote = TestObjectFactory.CreateQuote();
            dbContext.Quotes.Add(quote);
            dbContext.SaveChanges();

            var quoteInDb = dbContext.Quotes.Find(quote.Id);

            Assert.IsNotNull(quoteInDb);
            Assert.AreEqual(quoteInDb.Content, quote.Content);
            Assert.AreEqual(quoteInDb.Author, quote.Author);
        }

        [TestMethod]
        public void TestAuthor_ShouldAddEntityToDbSetAndDbCorrectly()
        {
            QuoteQuizContext dbContext = new QuoteQuizContext();

            var author = TestObjectFactory.CreateAuthor();
            dbContext.Authors.Add(author);
            dbContext.SaveChanges();

            var authorInDb = dbContext.Authors.Find(author.Id);

            Assert.IsNotNull(authorInDb);
            Assert.AreEqual(authorInDb.Name, author.Name);
        }
    }
}
