using System;
using System.Linq.Expressions;

using FamousQuoteQuiz.Models;

namespace FamousQuoteQuiz.Services.DTOs
{
    public class AuthorDTO
    {
        public static Expression<Func<Author, AuthorDTO>> MapToDTO
        {
            get
            {
                return x => new AuthorDTO
                {
                    Name = x.Name
                };
            }
        }

        public string Name { get; set; }
    }
}
