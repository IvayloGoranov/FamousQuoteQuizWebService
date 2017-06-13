using System.ComponentModel.DataAnnotations;

namespace FamousQuoteQuiz.Models
{
    public abstract class BaseModel<TKey>
    {
        [Key]
        public virtual TKey Id { get; set; }
    }
}
