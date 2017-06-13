using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FamousQuoteQuiz.Models
{
    public class Author : BaseModel<int>
    {
        private ICollection<Quote> quotes;

        public Author()
        {
            this.quotes = new HashSet<Quote>();
        }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} name must be between {1} and {2} characters long.",
            MinimumLength = 1)]
        public string Name { get; set; }

        public virtual ICollection<Quote> Quotes
        {
            get
            {
                return this.quotes;
            }

            set
            {
                this.quotes = value;
            }
        }
    }
}
