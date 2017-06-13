using Microsoft.VisualStudio.TestTools.UnitTesting;

using FamousQuoteQuiz.Tests.Common;

namespace FamousQuoteQuiz.Models.Tests
{
    [TestClass]
    public class AuthorTest : TestBase
    {
        [TestMethod]
        public void CreateAuthorWithMissingName_ShouldThrow()
        {
            var author = TestObjectFactory.CreateAuthor();
            author.Name = null;
            this.ValidateEntity(author);
        }
    }
}
