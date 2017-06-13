using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FamousQuoteQuiz.Models.Tests
{
    public class TestBase
    {
        protected void ValidateEntity(object entity)
        {
            var context = new ValidationContext(entity, null, null);
            var results = new List<ValidationResult>();

            var actualValidationResult = Validator.TryValidateObject(entity, context, results, true);
            var expectedValidationResult = false;

            Assert.AreEqual(expectedValidationResult, actualValidationResult);
        }
    }
}
