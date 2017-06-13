using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FamousQuoteQuiz.Models;
using FamousQuoteQuiz.Data.Repositories;
using FamousQuoteQuiz.Tests.Common;

namespace FamousQuoteQuiz.Services.Tests
{
    [TestClass]
    public class QuoteServiceTest
    {
        private QuotesService service;
        private Quote quoteTestObject;
        private IRepository<Quote> quoteRepository;

        [TestInitialize]
        public void Init()
        {
            this.quoteTestObject = TestObjectFactory.CreateQuote();
            quoteTestObject.Id = 1;

            var quoteTestData = new List<Quote>();
            quoteTestData.Add(this.quoteTestObject);

            this.quoteRepository = TestObjectFactory.CreateRepository(quoteTestData);

            this.service = new QuotesService(this.quoteRepository);
        }

        [TestMethod]
        public async Task TestFind_ValidQuoteId_ShouldReturnQuoteFromDB()
        {
            var quoteToFind = await this.service.GetQuoteById(1);

            Assert.AreEqual(quoteToFind, this.quoteTestObject);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task TestFind_MissingQuoteId_ShouldThrow()
        {
            var quoteToFind = await this.service.GetQuoteById(2);
            System.Console.WriteLine("I am here");
        }
    }
}
