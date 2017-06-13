using Microsoft.VisualStudio.TestTools.UnitTesting;

using FamousQuoteQuiz.Tests.Common;

namespace FamousQuoteQuiz.Models.Tests
{
    [TestClass]
    public class QuoteTest : TestBase
    {
        [TestMethod]
        public void CreateQuoteWithMissingContent_ShouldThrow()
        {
            var quote = TestObjectFactory.CreateQuote();
            quote.Content = null;
            this.ValidateEntity(quote);
        }
    }
}
