using System;
using System.Linq.Expressions;

using FamousQuoteQuiz.Models;

namespace FamousQuoteQuiz.Services.DTOs
{
    public class QuoteDTO
    {
        public static Expression<Func<Quote, QuoteDTO>> MapToDTO
        {
            get
            {
                return x => new QuoteDTO
                {
                    Id = x.Id,
                    Content = x.Content,
                    Author = x.Author.Name
                };
            }
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }
    }
}
