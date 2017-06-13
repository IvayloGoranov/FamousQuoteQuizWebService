using System;

namespace FamousQuoteQuiz.Data
{
    public class UnpopulatedDbException : Exception
    {
        public UnpopulatedDbException()
          : base()
        {
        }

        public UnpopulatedDbException(string message)
          : base(message)
        {
        }

        public UnpopulatedDbException(string message, Exception innerException)
          : base(message, innerException)
        {
        }
    }
}
